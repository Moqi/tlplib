using UnityEngine;

namespace Utils {
  public static class PlayerPrefsExts {
    public static void SetIntAndSave(string name, int value) {
      PlayerPrefs.SetInt(name, value);
      PlayerPrefs.Save();
    }

    public static void SetFloatAndSave(string name, float value) {
      PlayerPrefs.SetFloat(name, value);
      PlayerPrefs.Save();
    }

    public static void SetStringAndSave(string name, string value) {
      PlayerPrefs.SetString(name, value);
      PlayerPrefs.Save();
    }
  }
}
