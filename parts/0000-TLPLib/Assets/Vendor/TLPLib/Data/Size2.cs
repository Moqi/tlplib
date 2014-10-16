namespace com.tinylabproductions.TLPLib.Data {
  public struct Size2 {
    public readonly int x, y;

    public Size2(int x, int y) {
      this.x = x;
      this.y = y;
    }

    public int index(int x, int y) { return x + y * this.x; }
    public int linearSize { get { return x * y; } }

    public override string ToString() { return string.Format("{0}x{1}", x, y); }
  }
}
