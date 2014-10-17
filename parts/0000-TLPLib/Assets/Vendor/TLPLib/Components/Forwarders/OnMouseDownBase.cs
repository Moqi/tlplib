using game_general.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Vendor.TLPLib.Components.Forwarders {
  public abstract class OnMouseDownBase : MonoBehaviour {
    private bool uguiBlocks;

    protected void init(bool uguiBlocks) 
    { this.uguiBlocks = uguiBlocks; }

    [UsedImplicitly]
    private void OnMouseDown() {
      if (uguiBlocks && EventSystem.current.IsPointerOverGameObject())
        return;

      mouseDown();
    }

    protected abstract void mouseDown();
  }
}
