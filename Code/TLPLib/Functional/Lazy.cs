using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public interface Lazy<out A> {
    A get { get; }
    bool initialized { get; }
  }

  public class LazyImpl<A> : Lazy<A> {
    public bool initialized { get; private set; }

    private A obj;
    private readonly Fn<A> initializer;

    public LazyImpl(Fn<A> initializer) {
      initialized = false;
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
