using System;

namespace com.tinylabproductions.TLPLib.Tween {
  /** TweenFacade helper class to make tweens type-safe **/
  public static class TF {
    /** Name of the property you need to pass to go tween **/
    public const string Prop = "value";

    public static TweenFacade<A> a<A>(Fn<A> getter, Act<A> setter) {
      return new TweenFacade<A>(getter, setter);
    }
  }

  public class TweenFacade<A> {
    private readonly Fn<A> getter;
    private readonly Act<A> setter;

    public TweenFacade(Fn<A> getter, Act<A> setter) {
      this.getter = getter;
      this.setter = setter;
    }

    public A value {
      get { return getter(); }
      set { setter(value); }
    }
  }
}
