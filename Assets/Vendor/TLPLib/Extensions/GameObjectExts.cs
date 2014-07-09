using System;
using com.tinylabproductions.TLPLib.Concurrent;
using UnityEngine;
using Coroutine = com.tinylabproductions.TLPLib.Concurrent.Coroutine;
using Object = UnityEngine.Object;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class GameObjectExts {
    public static void changeAlpha(this GameObject go, float alpha) {
      foreach (var childRenderer in go.GetComponentsInChildren<Renderer>()) {
        var material = childRenderer.material;
        var c = material.color;
        material.color = new Color(c.r, c.g, c.b, alpha);
      }
    }

    public static void doRecursively(this GameObject go, Act<GameObject> act) {
      act(go);
      var t = go.transform;
      for (var idx = 0; idx < t.childCount; idx++)
        t.GetChild(idx).gameObject.doRecursively(act);
    }

    public static void setLayerRecursively(this GameObject go, int layer) {
      go.doRecursively(o => o.layer = layer);
    }

    public static void replaceWith(this GameObject go, GameObject replacement) {
      replacement.transform.parent = go.transform.parent;
      replacement.transform.position = go.transform.position;
      replacement.transform.rotation = go.transform.rotation;
      replacement.transform.localScale = go.transform.localScale;
      Object.Destroy(go);
    }

    public static Coroutine everyFrame(this GameObject go, Fn<bool> f) {
      var behaviour =
        go.GetComponent<CoroutineHelperBehaviour>() ??
        go.AddComponent<CoroutineHelperBehaviour>();
      return ASync.EveryFrame(behaviour, f);
    }

    public static Coroutine everyFrame(this GameObject go, Act a) {
      return go.everyFrame(() => { a(); return true; });
    }
  }
}
