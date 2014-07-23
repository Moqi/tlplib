using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Logger;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.tinylabproductions.TLPLib.Threads {
  /* Helper class to queue things from other threads to be ran on the main
   * thread. */
  public class OnMainThread {
    private static readonly LinkedList<Act> actions = new LinkedList<Act>();

    /* Initialization. */
    static OnMainThread() {
      var go = new GameObject("On Main Thread Runner");
      Object.DontDestroyOnLoad(go);
      var behaviour = go.AddComponent<OnMainThreadBehaviour>();
      behaviour.init(onUpdate);
    }

    /* Explicit initialization. */
    public static void init() {}

    /* Run the given action in the main thread. */
    public static void run(Act action) 
    { lock (actions) { actions.AddLast(action); } }

    private static void onUpdate() {
      // This isn't thread safe, but we can live with that.
      if (actions.Count == 0) return;
      lock (actions) {
        var current = actions.First;
        while (current != null) {
          try { current.Value(); }
          catch (Exception e) { Log.error(e); }
          current = current.Next;
        }
        actions.Clear();
      }
    }
  }
}
