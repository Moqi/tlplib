using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Logger {
  public static class Log {
    public const bool isDebug = 
#if UNITY_EDITOR
      true;
#else
      false;
#endif

    public static void debug(object o) { if (isDebug) Debug.Log("[DEBUG]> " + o); }
    public static void debug(Fn<object> o) { if (isDebug) debug(o()); }
    public static void info(object o) { Debug.LogWarning("[INFO]> " + o); }
    public static void warn(object o) { Debug.LogWarning("[WARN]> " + o); }
    public static void error(Exception ex) { Debug.LogException(ex); }
    public static void error(object o) { Debug.LogError("[ERROR]> " + o); }
  }
}
