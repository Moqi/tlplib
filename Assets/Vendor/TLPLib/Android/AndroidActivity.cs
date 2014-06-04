#if UNITY_ANDROID
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Android {
  public static class AndroidActivity {
    private static readonly AndroidJavaClass unityPlayer;
    private static readonly AndroidJavaClass bridge;
    public static readonly AndroidJavaObject current;
    public static AndroidJavaObject activity { get { return current; } }
    
    static AndroidActivity() {
      if (Application.isEditor) return;
      unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      //bridge = new AndroidJavaClass("com.tinylabproductions.tlplib.Bridge");
      current = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }

    public static void sharePNG(string path, string title, string sharerText) {
      bridge.CallStatic("sharePNG", path, title, sharerText);
    }
  }
}
#endif