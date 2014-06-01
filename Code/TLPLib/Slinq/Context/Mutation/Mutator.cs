using com.tinylabproductions.TLPLib.Functional;

namespace Smooth.Slinq.Context {
	public delegate void Mutator<T, C>(ref C context, out Option<T> next);
}