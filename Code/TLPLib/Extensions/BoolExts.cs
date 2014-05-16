using System;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class BoolExts {
    public static Option<T> opt<T>(this bool condition, Fn<T> value) {
      return condition ? F.some(value()) : F.none<T>();
    }
  }
}
