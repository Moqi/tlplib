namespace com.tinylabproductions.TLPLib.Functional {
  public struct Unit {
    public static Unit instance { get { return new Unit(); } }
    public override string ToString() { return "()"; }
  }
}
