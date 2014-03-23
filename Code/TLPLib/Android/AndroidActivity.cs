#if UNITY_ANDROID
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Android {
  public static class AndroidActivity {
    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject activity;

    public static AndroidJavaObject current { get {
      if (unityPlayer == null) 
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      if (activity == null)
        activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
      return activity;
    } }
  }
}
#endif