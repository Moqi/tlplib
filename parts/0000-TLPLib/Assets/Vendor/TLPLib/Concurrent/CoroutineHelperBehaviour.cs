using UnityEngine;

namespace com.tinylabproductions.TLPLib.Concurrent {
  public class CoroutineHelperBehaviour : MonoBehaviour {
    public delegate void OnPause(bool paused);
    public event OnPause onPause;

    public delegate void OnQuit();
    public event OnQuit onQuit;

    internal void OnApplicationPause(bool paused) {
      if (onPause != null) onPause(paused);
    }

    internal void OnApplicationQuit() {
      if (onQuit != null) onQuit();
    }
  }
}
