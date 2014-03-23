namespace com.tinylabproductions.TLPLib.Extensions {
  public static class ArrayExts {
    public static A[] concat<A>(this A[] a, A[] b) {
      var self = new A[a.Length + b.Length];
      a.CopyTo(self, 0);
      b.CopyTo(self, a.Length);
      return self;
    }
  }
}
