using System;
using com.tinylabproductions.TLPLib.Components;
using com.tinylabproductions.TLPLib.Components.Forwarders;
using com.tinylabproductions.TLPLib.Concurrent;
using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Reactive;
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
      go.transform.doRecursively(t => act(t.gameObject));
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
      return ASync.EveryFrame(go, f);
    }

    public static Coroutine everyFrame(this GameObject go, Act a) {
      return go.everyFrame(() => { a(); return true; });
    }

    public static IObservable<Vector2> mouseDrag(this GameObject go) {
      return (
        go.GetComponent<DragObservable>() ??
        go.AddComponent<DragObservable>()
      ).dragDelta;
    }

    public static IObservable<Vector2> scrollWheel(this GameObject go) {
      return (
        go.GetComponent<ScrollWheelObservable>() ??
        go.AddComponent<ScrollWheelObservable>()
      ).scrollDelta;
    }

    public static IObservable<Unit> onMouseDown(this GameObject go, bool uguiBlocks=true) {
      return go.AddComponent<OnMouseDownForwarder>().init(uguiBlocks).onMouseDown;
    }

    public static IObservable<Vector3> onMouseDown(
      this GameObject go, Camera camera, float raycastDistance=1000, 
      bool uguiBlocks=true
    ) {
      return go.AddComponent<OnMouseDownCoordsForwarder>().
        init(uguiBlocks, camera, raycastDistance).onMouseDown;
    }

    public static IObservable<Unit> doubleClick(
      this GameObject go, float intervalS = 0.25f, bool uguiBlocks = true
    ) { return go.onMouseDown(uguiBlocks).withinTimeframe(2, intervalS).discardValue(); }
  }
}
