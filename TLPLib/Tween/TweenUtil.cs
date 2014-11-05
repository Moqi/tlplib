using System;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Tween {
  public static class TweenUtil {
    public delegate void SetCurrentTween(AbstractGoTween tween);

    /* Allows you to have only one running tween per closure. */
    public static void withTween(Act<SetCurrentTween> runTween) {
      var current = F.none<AbstractGoTween>();
      runTween(tween => {
        current.each(_ => _.destroy());
        current = F.some(tween);
      });
    }
  }
}
