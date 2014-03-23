using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.tinylabproductions.TLPLib.Concurrent {
  public static class Concurrent {
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
  }
}
