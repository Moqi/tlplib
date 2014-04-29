using System;
using System.Collections;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Reactive;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.tinylabproductions.TLPLib.Concurrent {
  public static class ASync {
    private static CoroutineHelperBehaviour behaviour { get {
      const string name = "Coroutine Helper";
      var go = GameObject.Find(name);
      if (go == null) {
        go = new GameObject(name);
        Object.DontDestroyOnLoad(go);
        go.AddComponent<CoroutineHelperBehaviour>();
      }
      return go.GetComponent<CoroutineHelperBehaviour>();
    } }

    public static Future<A> StartCoroutine<A>(
      Func<Promise<A>, IEnumerator> coroutine
    ) {
      var f = new FutureImpl<A>();
      behaviour.StartCoroutine(coroutine(f));
      return f;
    }

    public static Coroutine StartCoroutine(IEnumerator coroutine) {
      return behaviour.StartCoroutine(coroutine);
    }

    public static Coroutine WithDelay(float seconds, Action action) {
      return behaviour.StartCoroutine(WithDelayEnumerator(seconds, action));
    }

    private static IEnumerator WithDelayEnumerator(
      float seconds, Action action
    ) {
      yield return new WaitForSeconds(seconds);
      action();
    }

    public static IObservable<bool> onAppPause 
      { get { return behaviour.onPause; } }

    public static IObservable<Unit> onAppQuit
      { get { return behaviour.onQuit; } }

    /**
     * Takes a function that transforms an element into a future and 
     * applies it to all elements in given sequence.
     * 
     * However instead of applying all elements concurrently it waits
     * for the future from previous element to complete before applying
     * the next element.
     * 
     * Returns reactive value that can be used to observe current stage
     * of the application.
     **/
    public static IRxVal<Option<Try<B>>> inAsyncSeq<A, B>(
      this IEnumerable<A> enumerable, Fn<A, Future<B>> asyncAction
    ) {
      var rxRef = RxRef.a(F.none<Try<B>>());
      inAsyncSeq(enumerable.GetEnumerator(), rxRef, asyncAction);
      return rxRef;
    }

    private static void inAsyncSeq<A, B>(
      IEnumerator<A> e, IRxRef<Option<Try<B>>> rxRef, 
      Fn<A, Future<B>> asyncAction
    ) {
      if (! e.MoveNext()) return;
      asyncAction(e.Current).onComplete(b => {
        rxRef.value = F.some(b);
        inAsyncSeq(e, rxRef, asyncAction);
      });
    }
  }
}
