using System;
using System.Collections;
using com.tinylabproductions.TLPLib.Functional;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.tinylabproductions.TLPLib.Concurrent {
  public static class ASync {
    private static CoroutineHelperBehaviour coroutineHelper(GameObject go) {
      return 
        go.GetComponent<CoroutineHelperBehaviour>() ?? 
        go.AddComponent<CoroutineHelperBehaviour>();
    }

    private static CoroutineHelperBehaviour _behaviour;

    public static event CoroutineHelperBehaviour.OnPause onAppPause;
    public static event CoroutineHelperBehaviour.OnQuit onAppQuit;

    private static CoroutineHelperBehaviour behaviour { get {
      if (_behaviour == null) { 
        const string name = "Coroutine Helper";
        var go = new GameObject(name);
        Object.DontDestroyOnLoad(go);
        _behaviour = coroutineHelper(go);
        _behaviour.onPause += paused => F.opt(onAppPause).each(a => a(paused));
        _behaviour.onQuit += () => F.opt(onAppQuit).each(a => a());
      }
      return _behaviour;
    } }

    public static void init() { var _ = behaviour; }

    public static Future<A> StartCoroutine<A>(
      Func<Promise<A>, IEnumerator> coroutine
    ) {
      var f = new FutureImpl<A>();
      behaviour.StartCoroutine(coroutine(f));
      return f;
    }

    public static Coroutine StartCoroutine(IEnumerator coroutine) {
      return new Coroutine(behaviour, coroutine);
    }

    [Obsolete("Use Coroutine#stop")]
    public static void StopCoroutine(IEnumerator enumerator) {
      behaviour.StopCoroutine(enumerator);
    }

    [Obsolete("Use Coroutine#stop")]
    public static void StopCoroutine(Coroutine coroutine) {
      coroutine.stop();
    }

    public static Coroutine WithDelay(float seconds, Act action) {
      return WithDelay(seconds, behaviour, action);
    }

    public static Coroutine WithDelay(
      float seconds, MonoBehaviour behaviour, Act action
    ) {
      var enumerator = WithDelayEnumerator(seconds, action);
      return new Coroutine(behaviour, enumerator);
    }

    public static Coroutine NextFrame(Action action) {
      return NextFrame(behaviour, action);
    }

    public static Coroutine NextFrame(GameObject gameObject, Action action) {
      return NextFrame(coroutineHelper(gameObject), action);
    }

    public static Coroutine NextFrame(MonoBehaviour behaviour, Action action) {
      var enumerator = NextFrameEnumerator(action);
      return new Coroutine(behaviour, enumerator);
    }

    public static Coroutine AfterXFrames(
      int framesToSkip, Action action
    ) { return AfterXFrames(behaviour, framesToSkip, action); }

    public static Coroutine AfterXFrames(
      MonoBehaviour behaviour, int framesToSkip, Action action
    ) {
      return EveryFrame(behaviour, () => {
        if (framesToSkip <= 0) {
          action();
          return false;
        }
        else {
          framesToSkip--;
          return true;
        }
      });
    }

    public static void NextPostRender(Camera camera, Act action) {
      NextPostRender(camera, 1, action);
    }

    public static void NextPostRender(Camera camera, int afterFrames, Act action) {
      var pr = camera.gameObject.AddComponent<NextPostRenderBehaviour>();
      pr.init(action, afterFrames);
    }

    /* Do thing every frame until f returns false. */
    public static Coroutine EveryFrame(Fn<bool> f) {
      return EveryFrame(behaviour, f);
    }

    /* Do thing every frame until f returns false. */
    public static Coroutine EveryFrame(GameObject go, Fn<bool> f) {
      return EveryFrame(coroutineHelper(go), f);
    }

    /* Do thing every frame until f returns false. */
    public static Coroutine EveryFrame(MonoBehaviour behaviour, Fn<bool> f) {
      var enumerator = EveryWaitEnumerator(null, f);
      return new Coroutine(behaviour, enumerator);
    }

    /* Do thing every X seconds until f returns false. */
    public static Coroutine EveryXSeconds(float seconds, Fn<bool> f) {
      return EveryXSeconds(seconds, behaviour, f);
    }

    /* Do thing every X seconds until f returns false. */
    public static Coroutine EveryXSeconds(float seconds, GameObject go, Fn<bool> f) {
      return EveryXSeconds(seconds, coroutineHelper(go), f);
    }

    /* Do thing every X seconds until f returns false. */
    public static Coroutine EveryXSeconds(float seconds, MonoBehaviour behaviour, Fn<bool> f) {
      var enumerator = EveryWaitEnumerator(new WaitForSeconds(seconds), f);
      return new Coroutine(behaviour, enumerator);
    }

    /* Do async WWW request. Completes with WWWException if WWW fails. */
    public static Future<WWW> www(Fn<WWW> createWWW) {
      var f = new FutureImpl<WWW>();
      StartCoroutine(WWWEnumerator(createWWW(), f));
      return f;
    }

    public static IEnumerator WWWEnumerator(WWW www, Promise<WWW> promise) {
      yield return www;
      if (String.IsNullOrEmpty(www.error))
        promise.completeSuccess(www);
      else
        promise.completeError(new WWWException(www));
    }

    public static IEnumerator WithDelayEnumerator(
      float seconds, Act action
    ) {
      yield return new WaitForSeconds(seconds);
      action();
    }

    public static IEnumerator NextFrameEnumerator(Action action) {
      yield return null;
      action();
    }

    public static IEnumerator EveryWaitEnumerator(WaitForSeconds wait, Fn<bool> f) {
      while (f()) yield return wait;
    }
  }
}
