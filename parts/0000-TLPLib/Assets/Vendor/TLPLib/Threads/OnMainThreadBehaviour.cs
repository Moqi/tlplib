using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Threads {
  class OnMainThreadBehaviour : MonoBehaviour {
    private Act runOnUpdate;

    public void init(Act runOnUpdate) { this.runOnUpdate = runOnUpdate; }

    internal void Update() { runOnUpdate(); }
  }
}
