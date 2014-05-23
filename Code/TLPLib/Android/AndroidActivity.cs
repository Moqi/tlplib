#if UNITY_ANDROID
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Android {
  public static class AndroidActivity {
    private readonly static AndroidJavaClass unityPlayer = 
      new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private readonly static AndroidJavaClass bridge =
      new AndroidJavaClass("com.tinylabproductions.tlplib.Bridge");
    public readonly static AndroidJavaObject current =
      unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

    public static void sharePNG(string path, string title, string sharerText) {
      bridge.CallStatic("sharePNG", path, title, sharerText);
    }
  }
}
#endif