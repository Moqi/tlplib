using System;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class FuncExts {
    public static Act<A> andThen<A, B>(this Fn<A, B> f, Act<B> a) {
      return value => a(f(value));
    }

    public static Fn<A, C> andThen<A, B, C>(this Fn<A, B> f1, Fn<B, C> f2) {
      return value => f2(f1(value));
    }
  }
}
