﻿using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public static class LazyExts {
    public static Lazy<B> map<A, B>(this Lazy<A> lazy, Fn<A, B> mapper) {
      return F.lazy(() => mapper(lazy.get));
    }
  }

  public interface Lazy<out A> {
    bool initialized { get; }
    A get { get; }
    // For those cases where we want it happen as a side effect.
    A getM();
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

    public A getM() { return get; }
  }
}
