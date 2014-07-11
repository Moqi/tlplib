using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Concurrent {
  class NextPostRenderBehaviour : MonoBehaviour {
    private Act action;

    public void init(Act action) {
      this.action = action;
    }

    internal void OnPostRender() {
      action();
      Destroy(this);
    }
  }
}
