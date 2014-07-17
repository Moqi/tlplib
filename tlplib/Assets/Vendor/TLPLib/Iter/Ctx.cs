using com.tinylabproductions.TLPLib.Functional;
using Smooth.Pools;

namespace com.tinylabproductions.TLPLib.Iter {
  public interface ICtx {
    void release();
  }

  public static class Ctx {
    public static ICtx a<P1>(P1 p1) {
      return Ctx<P1, Unit, Unit>.pool.Borrow().set(p1, F.unit, F.unit);
    }

    public static ICtx a<P1, P2>(P1 p1, P2 p2) {
      return Ctx<P1, P2, Unit>.pool.Borrow().set(p1, p2, F.unit);
    }

    public static ICtx a<P1, P2, P3>(P1 p1, P2 p2, P3 p3) {
      return Ctx<P1, P2, P3>.pool.Borrow().set(p1, p2, p3);
    }
  }

  public class Ctx<P1, P2, P3> : ICtx {
    public static Pool<Ctx<P1, P2, P3>> pool = new Pool<Ctx<P1, P2, P3>>(
      () => new Ctx<P1, P2, P3>(),
      ctx => ctx.reset()
    );

    public P1 _1 { get; private set; }
    public P2 _2 { get; private set; }
    public P3 _3 { get; private set; }

    internal ICtx set(P1 p1, P2 p2, P3 p3) {
      _1 = p1;
      _2 = p2;
      _3 = p3;
      return this;
    }

    public void reset() {
      _1 = default(P1);
      _2 = default(P2);
      _3 = default(P3);
    }

    public void release() { pool.Release(this); }
  }
}
