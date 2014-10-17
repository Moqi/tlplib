using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Reactive;

namespace Assets.Vendor.TLPLib.Components.Forwarders {
  public class OnMouseDownForwarder : OnMouseDownBase {
    private readonly Subject<Unit> _onMouseDown = new Subject<Unit>();
    public IObservable<Unit> onMouseDown { get { return _onMouseDown; } }

    public new OnMouseDownForwarder init(bool ignoreIfUGUIClicked) {
      base.init(ignoreIfUGUIClicked);
      return this;
    }

    protected override void mouseDown() { _onMouseDown.push(F.unit); }
  }
}
