using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Logger {
  public static class Log {
    public static void debug(object o) { Debug.Log(o); }
    public static void warn(object o) { Debug.LogWarning(o); }
    public static void error(Exception ex) { Debug.LogException(ex); }
    public static void error(object o) { Debug.LogError(o); }
  }
}
