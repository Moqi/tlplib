using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public static class LazyExts {
    public static Lazy<B> map<A, B>(this Lazy<A> lazy, Fn<A, B> mapper) {
      return F.lazy(() => mapper(lazy.get));
    }
  }

  public interface Lazy<out A> {
    A get { get; }
  }

  public class LazyImpl<A> : Lazy<A> {
    private A obj;
    private bool initialized;
    private readonly Fn<A> initializer;

    public LazyImpl(Fn<A> initializer) {
      this.initializer = initializer;
    }

    public A get { get {
      if (! initialized) {
        obj = initializer();
        initialized = true;
      }
      return obj;
    } }
  }
}
