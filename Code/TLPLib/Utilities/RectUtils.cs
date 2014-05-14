using UnityEngine;

namespace com.tinylabproductions.TLPLib.Utilities {
  public static class RectUtils {
    private static float sw { get { return Screen.width; } }
    private static float sh { get { return Screen.height; } }

    /** Create rect that has values from percentages of screen. **/
    public static Rect percent(float left, float top, float width, float height) {
      return new Rect(sw * left, sh * top, sw * width, sh * height);
    }

    /** Convert absolute rect to percentage rect. **/
    public static Rect absoluteToPercentage(this Rect pRect) {
      return new Rect(
        pRect.xMin / sw, pRect.yMin / sh, pRect.width / sw, pRect.height / sh
      );
    }

    /** Create rect that has values from percentages of screen. **/
    public static Rect relPercent(float left, float leftEnd, float top, float topEnd) {
      return percent(left, leftEnd - left, top, topEnd - top);
    }

    public static Rect with(
      this Rect rect, float? left = null, float? top = null,
      float? width = null, float? height = null
    ) {
      return new Rect(
        left ?? rect.xMin, top ?? rect.yMin,
        width ?? rect.width, height ?? rect.height
      );
    }
  }
}
