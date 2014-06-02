using System.Runtime.InteropServices;
using com.tinylabproductions.TLPLib.Functional;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class VectorExts {
    public static Vector2 with2(
      this Vector2 v, 
      Option<float> x = new Option<float>(), 
      Option<float> y = new Option<float>()
    ) {
      return new Vector3(x.getOrElse(v.x), y.getOrElse(v.y));
    }

    public static Vector3 with3(
      this Vector3 v, 
      Option<float> x = new Option<float>(),
      Option<float> y = new Option<float>(),
      Option<float> z = new Option<float>()
    ) {
      return new Vector3(x.getOrElse(v.x), y.getOrElse(v.y), z.getOrElse(v.z));
    }
  }
}
