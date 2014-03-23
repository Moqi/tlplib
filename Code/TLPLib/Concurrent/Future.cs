using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Concurrent {
  /** Coroutine based future **/
  public interface Future<out A> {
    Option<A> value { get; }
    Future<B> map<B>(Fn<A, B> mapper);
    Future<B> flatMap<B>(Fn<A, Future<B>> mapper);
    Future<A> onComplete(Act<A> action);
  }

  /** Couroutine based promise **/
  public interface Promise<in A> {
    void complete(A v);
  }
  
  public static class CoFuture {
    public static Future<A> successful<A>(A value) {
      var f = new FutureImpl<A>();
      f.complete(value);
      return f;
    }
  }

  class FutureImpl<A> : Future<A>, Promise<A> {
    private readonly IList<Act<A>> listeners = new List<Act<A>>();

    private Option<A> _value = F.none<A>();
    public Option<A> value { get { return _value; } }

    public void complete(A v) {
      value.voidFold(
        () => _value = F.some(v), 
        _ => { throw new Exception("Promise is already completed with " + _); }
      );
      completed(v);
    }

    public Future<B> map<B>(Fn<A, B> mapper) {
      var p = new FutureImpl<B>();
      onComplete(v => p.complete(mapper(v)));
      return p;
    }

    public Future<B> flatMap<B>(Fn<A, Future<B>> mapper) {
      var p = new FutureImpl<B>();
      onComplete(v => mapper(v).onComplete(p.complete));
      return p;
    }

    public Future<A> onComplete(Act<A> action) {
      value.voidFold(() => listeners.Add(action), action);
      return this;
    }

    public void completed(A v) {
      foreach (var listener in listeners) listener(v);
      listeners.Clear();
    }
  }
}
