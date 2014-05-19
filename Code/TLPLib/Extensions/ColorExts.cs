using UnityEngine;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class ColorExts {
    public static Color withAlpha(this Color color, float alpha) {
      return new Color(color.r, color.g, color.b, alpha);
    }

    public static Color32 withAlpha(this Color32 color, int alpha) {
      return new Color(color.r, color.g, color.b, alpha);
    }
  }
}
