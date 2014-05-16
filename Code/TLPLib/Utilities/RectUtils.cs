using UnityEngine;

namespace com.tinylabproductions.TLPLib.Utilities {
  public static class RectUtils {
    /** Create rect that has values from percentages of screen. **/
    public static Rect percent(float left, float top, float width, float height) {
      var sw = Screen.width;
      var sh = Screen.height;
      return new Rect(sw * left, sh * top, sw * width, sh * height);
    }

    /** Create rect that has values from percentages of screen. **/
    public static Rect relPercent(float left, float leftEnd, float top, float topEnd) {
      var sw = Screen.width;
      var sh = Screen.height;
      var absLeft = sw * left;
      var absLeftEnd = sw * leftEnd;
      var absTop = sh * top;
      var absTopEnd = sh * topEnd;
      return new Rect(absLeft, absTop, absLeftEnd - absLeft, absTopEnd - absTop);
    }
  }
}
