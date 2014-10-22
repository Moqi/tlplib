using com.tinylabproductions.TLPLib.Annotations;
using com.tinylabproductions.TLPLib.Functional;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.tinylabproductions.TLPLib.Components.Forwarders {
  public abstract class OnMouseClickBase : MonoBehaviour {
    private bool uguiBlocks;
    private Option<Vector3> downPosition;

    protected void init(bool uguiBlocks) 
    { this.uguiBlocks = uguiBlocks; }

    [UsedImplicitly]
    private void OnMouseDown() {
      if (uguiBlocks && EventSystem.current.IsPointerOverGameObject())
        return;

      downPosition = F.some(Input.mousePosition);
    }

    [UsedImplicitly]
    private void Update() {
      if (downPosition.isEmpty) return;
      var startPos = downPosition.get;
      var diff = Input.mousePosition - startPos;
      if (diff.sqrMagnitude >= DragObservable.dragThresholdSqr) 
        downPosition = F.none<Vector3>();
    }

    [UsedImplicitly]
    private void OnMouseUp() {
      if (downPosition.isEmpty) return;
      if (uguiBlocks && EventSystem.current.IsPointerOverGameObject()) return;
      mouseClick();
    }

    protected abstract void mouseClick();
  }
}
