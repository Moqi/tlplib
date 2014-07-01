using System;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace com.tinylabproductions.TLPLib.Logger {
  public static class Log {
    public static readonly bool isDebug = Application.isEditor || Debug.isDebugBuild;

    public static void debug(object o) { if (isDebug) Debug.Log("[DEBUG]> " + o); }
    // Lazy debug with context. Context is required to not allocate a closure
    // for enclosed variables.
    public static void debug<Ctx>(Ctx ctx, Fn<Ctx, object> o) { if (isDebug) debug(o(ctx)); }
    public static void info(object o) { Debug.LogWarning("[INFO]> " + o); }
    public static void warn(object o) { Debug.LogWarning("[WARN]> " + o); }
    public static void error(Exception ex) { Debug.LogException(ex); }
    public static void error(object o) { Debug.LogError("[ERROR]> " + o); }

#if UNITY_EDITOR
    public static readonly string logfilePath;
    public static readonly StreamWriter logfile;

    static Log() {
      logfilePath = Application.temporaryCachePath + "/unity-editor-runtime.log";
      info("Editor Runtime Logfile: " + logfilePath);
      logfile = new StreamWriter(
        File.Open(logfilePath, FileMode.Append, FileAccess.Write, FileShare.Read)
      ) { AutoFlush = true };

      editor("\n\nLog opened at " + DateTime.Now + "\n\n");
    }

    public static void editor(object o) { logfile.WriteLine(o); }
#endif
  }
}
