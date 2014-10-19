using com.tinylabproductions.TLPLib.Reactive;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Components.Forwarders {
  public class OnMouseDownCoordsForwarder : OnMouseDownBase {
    private readonly Subject<Vector3> _onMouseDown = new Subject<Vector3>();
    public IObservable<Vector3> onMouseDown { get { return _onMouseDown; } }
    public new Camera camera { get; private set; }
    public float raycastDistance { get; private set; }

    public OnMouseDownCoordsForwarder init(
      bool ignoreIfUGUIClicked, Camera camera, float raycastDistance
    ) {
      init(ignoreIfUGUIClicked);
      this.camera = camera;
      this.raycastDistance = raycastDistance;
      return this;
    }

    protected override void mouseDown() {
      RaycastHit hit;
      var ray = camera.ScreenPointToRay(Input.mousePosition);
      if (collider.Raycast(ray, out hit, raycastDistance)) {
        _onMouseDown.push(hit.point);
      }
    }
  }
}
