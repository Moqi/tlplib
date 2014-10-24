using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Tween {
  /** TweenFacade helper class to make tweens type-safe **/
  public static class TF {
    /** Name of the property you need to pass to go tween **/
    public const string Prop = "value";

    public static TweenFacade<A> a<A>(Fn<A> getter, Act<A> setter) {
      return new TweenFacade<A>(getter, setter);
    }

    public static TweenFacade<Vector3> transformPosition(Transform t) 
    { return a(() => t.position, v => t.position = v); }
  }

  public struct TweenFacade<A> {
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

  public static class TweenCfg {
    public static TweenCfg<A> a<A>(TweenFacade<A> facade, GoTweenConfig config) {
      return new TweenCfg<A>(facade, config);
    }
  }

  public struct TweenCfg<A> {
    public readonly TweenFacade<A> facade;
    public readonly GoTweenConfig config;

    public TweenCfg(TweenFacade<A> facade, GoTweenConfig config) {
      this.facade = facade;
      this.config = config;
    }
  }
}
