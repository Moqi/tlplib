using com.tinylabproductions.TLPLib.Annotations;
using com.tinylabproductions.TLPLib.Functional;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.tinylabproductions.TLPLib.Components.Forwarders {
  public abstract class OnMouseClickBase : MonoBehaviour {
    private static Option<OnMouseClickBase> currentDown = Option<OnMouseClickBase>.None;
    public static void clear() { currentDown = Option<OnMouseClickBase>.None; }

    private bool uguiBlocks;

    protected void init(bool uguiBlocks) 
    { this.uguiBlocks = uguiBlocks; }

    [UsedImplicitly]
    private void OnMouseDown() {
      if (uguiBlocks && EventSystem.current.IsPointerOverGameObject())
        return;

      currentDown = new Option<OnMouseClickBase>(this);
    }

    [UsedImplicitly]
    private void OnMouseUp() {
      currentDown.each(cur => {
        if (cur != this) return;
        if (uguiBlocks && EventSystem.current.IsPointerOverGameObject())
          return;
        mouseClick();
      });
      clear();
    }

    protected abstract void mouseClick();
  }
}
