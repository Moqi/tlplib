using System;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class ArrayExts {
    public static A[] concat<A>(this A[] a, A[] b) {
      var self = new A[a.Length + b.Length];
      a.CopyTo(self, 0);
      b.CopyTo(self, a.Length);
      return self;
    }

    /**
     * Basically LINQ #Select, but for arrays. Needed because of performance/iOS
     * limitations of AOT.
     **/
    public static To[] map<From, To>(
      this From[] source, Func<From, To> mapper
    ) {
      var target = new To[source.Length];
      for (var i = 0; i < source.Length; i++) target[i] = mapper(source[i]);
      return target;
    }
  }
}
