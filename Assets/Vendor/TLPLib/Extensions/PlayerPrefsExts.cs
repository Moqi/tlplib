using com.tinylabproductions.TLPLib.Logger;
using UnityEngine;

namespace Utils {
  public static class PlayerPrefsExts {
    public static void SetIntAndSave(string name, int value) {
      Log.debug(string.Format("Storing PP {0}={1}", name, value));
      PlayerPrefs.SetInt(name, value);
      PlayerPrefs.Save();
    }

    public static void SetFloatAndSave(string name, float value) {
      Log.debug(string.Format("Storing PP {0}={1}", name, value));
      PlayerPrefs.SetFloat(name, value);
      PlayerPrefs.Save();
    }

    public static void SetStringAndSave(string name, string value) {
      Log.debug(string.Format("Storing PP {0}={1}", name, value));
      PlayerPrefs.SetString(name, value);
      PlayerPrefs.Save();
    }
  }
}
