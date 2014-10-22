using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Reactive;

namespace com.tinylabproductions.TLPLib.Components.Forwarders {
  public class OnMouseClickForwarder : OnMouseClickBase {
    private readonly Subject<Unit> _onMouseClick = new Subject<Unit>();
    public IObservable<Unit> onMouseClick { get { return _onMouseClick; } }

    public new OnMouseClickForwarder init(bool ignoreIfUGUIClicked) {
      base.init(ignoreIfUGUIClicked);
      return this;
    }

    protected override void mouseClick() { _onMouseClick.push(F.unit); }
  }
}
