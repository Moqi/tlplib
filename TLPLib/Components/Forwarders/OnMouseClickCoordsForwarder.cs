using com.tinylabproductions.TLPLib.Reactive;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Components.Forwarders {
  public class OnMouseClickCoordsForwarder : OnMouseClickBase {
    private readonly Subject<Vector3> _onMouseClick = new Subject<Vector3>();
    public IObservable<Vector3> onMouseClick { get { return _onMouseClick; } }
    public new Camera camera { get; private set; }
    public float raycastDistance { get; private set; }

    public OnMouseClickCoordsForwarder init(
      bool ignoreIfUGUIClicked, Camera camera, float raycastDistance
    ) {
      init(ignoreIfUGUIClicked);
      this.camera = camera;
      this.raycastDistance = raycastDistance;
      return this;
    }

    protected override void mouseClick() {
      RaycastHit hit;
      var ray = camera.ScreenPointToRay(Input.mousePosition);
      if (collider.Raycast(ray, out hit, raycastDistance)) {
        _onMouseClick.push(hit.point);
      }
    }
  }
}
