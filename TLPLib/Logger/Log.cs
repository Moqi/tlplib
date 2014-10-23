using System;
using System.Diagnostics;
using System.IO;
using com.tinylabproductions.TLPLib.Extensions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace com.tinylabproductions.TLPLib.Logger {
  public static class Log {
    [Conditional("DEBUG")]
    public static void debug(object o) {
      var str = "[DEBUG]> " + o;
      Debug.Log(str);
      file(str);
    }

    public static void info(object o) {
      var str = "[INFO]> " + o;
      Debug.LogWarning(str);
      file(str);
    }

    public static void warn(object o) {
      var str = "[WARN]> " + o;
      Debug.LogWarning(str);
    }

    public static void error(Exception ex) {
      Debug.LogException(ex);
      file("[ERROR]> Exception!");
      file(ex.ToString());
    }

    public static void error(object o) {
      var str = "[ERROR]> " + o;
      Debug.LogError(str);
      file(str);
    }

    [Conditional("DEBUG")]
    public static void file(object o) { FileLog.log(o); }
  }

  class FileLog {
    public static readonly string logfilePath;
    public static readonly StreamWriter logfile;
    
    static FileLog() {
      logfilePath = Application.temporaryCachePath + "/runtime.log";
      logfile = new StreamWriter(
        File.Open(logfilePath, FileMode.Append, FileAccess.Write, FileShare.Read)
      ) { AutoFlush = true };

      Log.info("Runtime Logfile: " + logfilePath);
      log("\n\nLog opened at " + DateTime.Now + "\n\n");
    }

    [Conditional("DEBUG")]
    public static void log(object o) { logfile.WriteLine(o); }
  }
}
