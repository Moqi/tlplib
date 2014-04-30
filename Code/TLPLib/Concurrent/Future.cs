using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Concurrent {
  /** Coroutine based future **/
  // If you make it covariant Unity mono runtime crashes randomly.
  public interface Future<A> {
    Option<Try<A>> value { get; }
    Future<B> map<B>(Fn<A, B> mapper);
    Future<B> flatMap<B>(Fn<A, Future<B>> mapper);
    CancellationToken onComplete(Act<Try<A>> action);
    CancellationToken onSuccess(Act<A> action);
    CancellationToken onFailure(Act<Exception> action);
  }

  /**
   * You can use this token to cancel a callback before future is completed.
   **/
  public interface CancellationToken {
    bool isCancelled { get; }
    // Returns true if cancelled or false if already cancelled before.
    bool cancel();
  }

  /** Couroutine based promise **/
  public interface Promise<in A> {
    /** Complete with value, exception if already completed. **/
    void complete(Try<A> v);
    void completeSuccess(A v);
    void completeError(Exception ex);
    /** Complete with value, return false if already completed. **/
    bool tryComplete(Try<A> v);
    bool tryCompleteSuccess(A v);
    bool tryCompleteError(Exception ex);
  }
  
  public static class Future {
    public static Future<A> successful<A>(A value) {
      var f = new FutureImpl<A>();
      f.completeSuccess(value);
      return f;
    }

    public static Future<A> unfullfiled<A>() {
      return new FutureImpl<A>();
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
      sourceFutures.eachWithIndex((f, idx) => {
        f.onSuccess(value => {
          results[idx] = value;
          completed++;
          if (completed == results.Length) future.tryCompleteSuccess(results);
        });
        f.onFailure(future.completeError);
      });
      return future;
    }

    /**
     * Returns result from the first future that completes.
     **/
    public static Future<A> firstOf<A>
    (this IEnumerable<Future<A>> enumerable) {
      var future = new FutureImpl<A>();
      enumerable.each(f => f.onComplete(v => future.tryComplete(v)));
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
      p.completeSuccess(Unit.instance);
    }

    public class FinishedCancellationToken : CancellationToken {
      private static FinishedCancellationToken _instance;

      public static FinishedCancellationToken instance { get {
        return _instance ?? (_instance = new FinishedCancellationToken());
      } }

      private FinishedCancellationToken() {}

      public bool isCancelled { get { return true; } }
      public bool cancel() { return false; }
    }
  }

  class FutureImpl<A> : Future<A>, Promise<A> {
    public class CancellationTokenImpl : CancellationToken {
      private readonly Act<Try<A>> action;
      private readonly FutureImpl<A> future;
      public bool isCancelled { get; private set; } 

      public CancellationTokenImpl(Act<Try<A>> action, FutureImpl<A> future) {
        this.action = action;
        this.future = future;
        isCancelled = false;
      }

      public bool cancel() {
        isCancelled = true;
        return future.cancel(action);
      }
    }

    private readonly IList<Act<Try<A>>> listeners = new List<Act<Try<A>>>();

    private Option<Try<A>> _value = F.none<Try<A>>();
    public Option<Try<A>> value { get { return _value; } }

    public void complete(Try<A> v) {
      if (! tryComplete(v))
        throw new Exception("Promise is already completed with " + value.get);
    }

    public void completeSuccess(A v) { complete(F.scs(v)); }

    public void completeError(Exception ex) { complete(F.err<A>(ex)); }

    public bool tryComplete(Try<A> v) {
      var ret = value.
        fold(() => { _value = F.some(v); return true; }, _ => false);
      completed(v);
      return ret;
    }

    public bool tryCompleteSuccess(A v) {
      return tryComplete(F.scs(v));
    }

    public bool tryCompleteError(Exception ex) {
      return tryComplete(F.err<A>(ex));
    }

    public Future<B> map<B>(Fn<A, B> mapper) {
      var p = new FutureImpl<B>();
      onComplete(t => t.voidFold(
        v => {
          try { p.completeSuccess(mapper(v)); }
          catch (Exception e) { p.completeError(e); }
        },
        p.completeError
      ));
      return p;
    }

    public Future<B> flatMap<B>(Fn<A, Future<B>> mapper) {
      var p = new FutureImpl<B>();
      onComplete(t => t.voidFold(
        v => {
          try { mapper(v).onComplete(p.complete); }
          catch (Exception e) { p.completeError(e); }
        },
        p.completeError
      ));
      return p;
    }

    public CancellationToken onComplete(Act<Try<A>> action) {
      return value.fold<CancellationToken>(() => {
        listeners.Add(action);
        return new CancellationTokenImpl(action, this);
      }, v => {
        action(v);
        return Future.FinishedCancellationToken.instance;
      });
    }

    public CancellationToken onSuccess(Act<A> action) {
      return onComplete(t => t.value.each(action));
    }

    public CancellationToken onFailure(Act<Exception> action) {
      return onComplete(t => t.exception.each(action));
    }

    public void completed(Try<A> v) {
      foreach (var listener in listeners) listener(v);
      listeners.Clear();
    }

    private bool cancel(Act<Try<A>> action) {
      return listeners.Remove(action);
    }
  }
}
