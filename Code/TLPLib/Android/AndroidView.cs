using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Concurrent;
using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Android {
#if UNITY_ANDROID
  public static class AndroidView {
    private const string FLAG_HIDE_NAVIGATION = "SYSTEM_UI_FLAG_HIDE_NAVIGATION";
    private const string FLAG_STABLE_LAYOUT = "SYSTEM_UI_FLAG_LAYOUT_STABLE";
    private const string FLAG_IMMERSIVE_STICKY = "SYSTEM_UI_FLAG_IMMERSIVE_STICKY";
    private const string FLAG_FULLSCREEN = "SYSTEM_UI_FLAG_FULLSCREEN";

    private static readonly AndroidJavaClass view;

    static AndroidView() {
      if (Application.isEditor) return;
      view = new AndroidJavaClass("android.view.View");
    }

    public static Future<bool> hideNavigationBar() {
      if (Application.isEditor) return Future.successful(false);

      Debug.Log("Trying to hide android navigation bar.");
      var activity = AndroidActivity.current;
      var future = new FutureImpl<bool>();
      activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
        try {
          int flags;
          try {
            flags =
              view.GetStatic<int>(FLAG_HIDE_NAVIGATION) |
              view.GetStatic<int>(FLAG_STABLE_LAYOUT) |
              view.GetStatic<int>(FLAG_IMMERSIVE_STICKY) |
              view.GetStatic<int>(FLAG_FULLSCREEN);
          }
          catch (Exception e1) {
            Debug.LogWarning("Failed to get immersive sticky mode flags: " + e1);
            flags =
              view.GetStatic<int>(FLAG_HIDE_NAVIGATION) |
              view.GetStatic<int>(FLAG_STABLE_LAYOUT) |
              view.GetStatic<int>(FLAG_FULLSCREEN);
          }

          var decor = activity.
            Call<AndroidJavaObject>("getWindow").
            Call<AndroidJavaObject>("getDecorView");
          decor.Call("setSystemUiVisibility", flags);
          Debug.Log("Hiding android navigation bar succeeded.");
          future.completeSuccess(true);
        }
        catch (Exception e2) {
          Debug.LogWarning(
            "Error while trying to hide navigation bar on android: " + e2
          );
          future.completeSuccess(false);
        }
      }));

      return future;
    }
  }
#endif
}