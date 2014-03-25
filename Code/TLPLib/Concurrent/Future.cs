using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;
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
  
  public static class Future {
    public static Future<A> successful<A>(A value) {
      var f = new FutureImpl<A>();
      f.complete(value);
      return f;
    }

    /**
     * Converts enumerable of futures into future of enumerable that is completed
     * when all futures complete.
     **/
    public static Future<A[]> sequence<A>(
      this IEnumerable<Future<A>> enumerable
    ) {
      var completed = 0u;
      var sourceFutures = enumerable.ToArray();
      var results = new A[sourceFutures.Length];
      var future = new FutureImpl<A[]>();
      sourceFutures.eachWithIndex((f, idx) => f.onComplete(value => {
        results[idx] = value;
        completed++;
        if (completed == results.Length) future.complete(results);
      }));
      return future;
    }

    public static Future<Unit> fromCoroutine(IEnumerator enumerator) {
      var f = new FutureImpl<Unit>();
      ASync.StartCoroutine(coroutineEnum(f, enumerator));
      return f;
    }

    private static IEnumerator coroutineEnum
    (Promise<Unit> p, IEnumerator enumerator) {
      yield return ASync.StartCoroutine(enumerator);
      p.complete(Unit.instance);
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
