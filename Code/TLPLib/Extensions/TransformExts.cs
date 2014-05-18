using System.Collections.Generic;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class TransformExts {
    public static void positionBetween(
      this Transform t, Vector3 start, Vector3 end, float width
    ) {
      var offset = end - start;
      var scale = new Vector3(width, offset.magnitude / 2.0f, width);
      var position = start + (offset / 2.0f);

      t.position = position;
      t.up = offset;
      t.localScale = scale;
    }

    public static void setPosition(
      this Transform t, float? x=null, float? y=null, float? z=null
    ) {
      t.position = t.position.with3(x, y, z);
    }

    public static IEnumerable<Transform> children(this Transform parent) {
      for (var idx = 0; idx < parent.childCount; idx++)
        yield return parent.GetChild(idx);
    }

    public static A addChild<A>(this Transform self, A child) 
    where A : Component {
      child.transform.parent = self;
      return child;
    }

    public static GameObject addChild(this Transform self, GameObject child) {
      self.addChild(child.transform);
      return child;
    }
  }
}
