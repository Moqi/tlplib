using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public interface Lazy<out A> {
    bool initialized { get; }
    A get { get; }
  }

  public class LazyImpl<A> : Lazy<A> {
    private A obj;
    public bool initialized { get; private set; }
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
