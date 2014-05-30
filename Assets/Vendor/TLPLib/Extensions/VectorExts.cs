using UnityEngine;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class VectorExts {
    public static Vector2 with2(
      this Vector2 v, float? x = null, float? y = null
    ) {
      return new Vector3(x ?? v.x, y ?? v.y);
    }

    public static Vector3 with3(
      this Vector3 v, float? x = null, float? y = null, float? z = null
    ) {
      return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
    }
  }
}
