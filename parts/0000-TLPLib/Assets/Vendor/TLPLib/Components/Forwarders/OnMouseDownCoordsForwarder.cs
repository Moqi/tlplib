using com.tinylabproductions.TLPLib.Reactive;
using game_general.Annotations;
using UnityEngine;

namespace Assets.Vendor.TLPLib.Components.Forwarders {
  public class OnMouseDownCoordsForwarder : MonoBehaviour {
    private readonly Subject<Vector3> _onMouseDown = new Subject<Vector3>();
    public IObservable<Vector3> onMouseDown { get { return _onMouseDown; } }
    public new Camera camera { get; private set; }
    public float raycastDistance { get; private set; }

    public OnMouseDownCoordsForwarder init(Camera camera, float raycastDistance) {
      this.camera = camera;
      this.raycastDistance = raycastDistance;
      return this;
    }

    [UsedImplicitly]
    private void OnMouseDown() {
      RaycastHit hit;
      var ray = camera.ScreenPointToRay(Input.mousePosition);
      if (collider.Raycast(ray, out hit, raycastDistance)){
        _onMouseDown.push(hit.point);
      }
    }
  }
}
