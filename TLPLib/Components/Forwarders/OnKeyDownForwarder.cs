using com.tinylabproductions.TLPLib.Annotations;
using com.tinylabproductions.TLPLib.Reactive;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Components.Forwarders {
  class OnKeyDownForwarder : MonoBehaviour {
    private readonly Subject<KeyCode> _onKeyDown = new Subject<KeyCode>();
    public IObservable<KeyCode> onKeyDown { get { return _onKeyDown; } }
    private KeyCode key;

    public OnKeyDownForwarder init(KeyCode key) {
      this.key = key;
      return this;
    }

    [UsedImplicitly]
    private void Update() {
      if (Input.GetKeyDown(key)) _onKeyDown.push(key);
    }
  }
}
