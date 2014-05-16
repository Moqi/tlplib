namespace com.tinylabproductions.TLPLib.Functional {
  public class Unit {
    public static readonly Unit instance = new Unit();
    private Unit() {}
    public override string ToString() { return "()"; }
  }
}
