using System;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class StringExts {
    public static Option<int> parseInt(this String str) {
      int output;
      return int.TryParse(str, out output).opt(() => output);
    }

    public static Option<float> parseFloat(this String str) {
      float output;
      return float.TryParse(str, out output).opt(() => output);
    }

    public static Option<double> parseDouble(this String str) {
      double output;
      return double.TryParse(str, out output).opt(() => output);
    }

    public static Option<bool> parseBool(this String str) {
      bool output;
      return bool.TryParse(str, out output).opt(() => output);
    }
  }
}
