using System;
using com.tinylabproductions.TLPLib.Functional;


namespace Smooth.Slinq.Context {
	[System.Flags]
	public enum ZipRemoveFlags {
		None = 0,
		Left = 1 << 0,
		Right = 1 << 1,
		Both = Left | Right,
	}

	#region Tpl
	
	public struct ZipContext<T2, C2, T, C> {
		
		#region Slinqs
		
		public static Slinq<Tpl<T, T2>, ZipContext<T2, C2, T, C>> Zip(Slinq<T, C> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags) {
			return new Slinq<Tpl<T, T2>, ZipContext<T2, C2, T, C>>(
				skip,
				remove,
				dispose,
				new ZipContext<T2, C2, T, C>(left, right, removeFlags));
		}
		
		#endregion

		#region Context
		
		private bool needsMove;
		private Slinq<T, C> left;
		private Slinq<T2, C2> right;
		private readonly ZipRemoveFlags removeFlags;
		
		#pragma warning disable 0414
		private BacktrackDetector bd;
		#pragma warning restore 0414

		private ZipContext(Slinq<T, C> left, Slinq<T2, C2> right, ZipRemoveFlags removeFlags) {
			this.needsMove = false;
			this.left = left;
			this.right = right;
			this.removeFlags = removeFlags;
			
			this.bd = BacktrackDetector.Borrow();
		}
		
		#endregion
		
		#region Delegates
		
		private static readonly Mutator<Tpl<T, T2>, ZipContext<T2, C2, T, C>> skip = Skip;
		private static readonly Mutator<Tpl<T, T2>, ZipContext<T2, C2, T, C>> remove = Remove;
		private static readonly Mutator<Tpl<T, T2>, ZipContext<T2, C2, T, C>> dispose = Dispose;
		
		private static void Skip(ref ZipContext<T2, C2, T, C> context, out Option<Tpl<T, T2>> next) {
			context.bd.DetectBacktrack();
			
			if (context.needsMove) {
				context.left.skip(ref context.left.context, out context.left.current);
				context.right.skip(ref context.right.context, out context.right.current);
			} else {
				context.needsMove = true;
			}
			
			if (context.left.current.isSome && context.right.current.isSome) {
				next = new Option<Tpl<T, T2>>(new Tpl<T, T2>(context.left.current.get, context.right.current.get));
			} else {
				next = new Option<Tpl<T, T2>>();
				context.bd.Release();
				if (context.left.current.isSome) {
					context.left.dispose(ref context.left.context, out context.left.current);
				} else if (context.right.current.isSome) {
					context.right.dispose(ref context.right.context, out context.right.current);
				}
			}
		}
		
		private static void Remove(ref ZipContext<T2, C2, T, C> context, out Option<Tpl<T, T2>> next) {
			context.bd.DetectBacktrack();
			
			context.needsMove = false;
			
			if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left) {
				context.left.remove(ref context.left.context, out context.left.current);
			} else {
				context.left.skip(ref context.left.context, out context.left.current);
			}
			
			if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right) {
				context.right.remove(ref context.right.context, out context.right.current);
			} else {
				context.right.skip(ref context.right.context, out context.right.current);
			}
			
			Skip(ref context, out next);
		}

		private static void Dispose(ref ZipContext<T2, C2, T, C> context, out Option<Tpl<T, T2>> next) {
			next = new Option<Tpl<T, T2>>();
			context.bd.Release();
			context.left.dispose(ref context.left.context, out context.left.current);
			context.right.dispose(ref context.right.context, out context.right.current);
		}
		
		#endregion
		
	}
	
	#endregion
	

	#region With selector
	
	public struct ZipContext<U, T2, C2, T, C> {

		#region Slinqs

		public static Slinq<U, ZipContext<U, T2, C2, T, C>> Zip(Slinq<T, C> left, Slinq<T2, C2> right, Fn<T, T2, U> selector, ZipRemoveFlags removeFlags) {
			return new Slinq<U, ZipContext<U, T2, C2, T, C>>(
				skip,
				remove,
				dispose,
				new ZipContext<U, T2, C2, T, C>(left, right, selector, removeFlags));
		}

		#endregion

		#region Selectors

		public static readonly Fn<T, T2, Tpl<T, T2>> Tpl = (t, t2) => new Tpl<T, T2>(t, t2);

		#endregion

		#region Context
		
		private bool needsMove;
		private Slinq<T, C> left;
		private Slinq<T2, C2> right;
		private readonly Fn<T, T2, U> selector;
		private readonly ZipRemoveFlags removeFlags;
		
		#pragma warning disable 0414
		private BacktrackDetector bd;
		#pragma warning restore 0414

		private ZipContext(Slinq<T, C> left, Slinq<T2, C2> right, Fn<T, T2, U> selector, ZipRemoveFlags removeFlags) {
			this.needsMove = false;
			this.left = left;
			this.right = right;
			this.selector = selector;
			this.removeFlags = removeFlags;
			
			this.bd = BacktrackDetector.Borrow();
		}

		#endregion
		
		#region Delegates
		
		private static readonly Mutator<U, ZipContext<U, T2, C2, T, C>> skip = Skip;
		private static readonly Mutator<U, ZipContext<U, T2, C2, T, C>> remove = Remove;
		private static readonly Mutator<U, ZipContext<U, T2, C2, T, C>> dispose = Dispose;

		private static void Skip(ref ZipContext<U, T2, C2, T, C> context, out Option<U> next) {
			context.bd.DetectBacktrack();
			
			if (context.needsMove) {
				context.left.skip(ref context.left.context, out context.left.current);
				context.right.skip(ref context.right.context, out context.right.current);
			} else {
				context.needsMove = true;
			}

			if (context.left.current.isSome && context.right.current.isSome) {
				next = new Option<U>(context.selector(context.left.current.get, context.right.current.get));
			} else {
				next = new Option<U>();
				context.bd.Release();
				if (context.left.current.isSome) {
					context.left.dispose(ref context.left.context, out context.left.current);
				} else if (context.right.current.isSome) {
					context.right.dispose(ref context.right.context, out context.right.current);
				}
			}
		}

		private static void Remove(ref ZipContext<U, T2, C2, T, C> context, out Option<U> next) {
			context.bd.DetectBacktrack();
			
			context.needsMove = false;

			if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left) {
				context.left.remove(ref context.left.context, out context.left.current);
			} else {
				context.left.skip(ref context.left.context, out context.left.current);
			}
			
			if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right) {
				context.right.remove(ref context.right.context, out context.right.current);
			} else {
				context.right.skip(ref context.right.context, out context.right.current);
			}

			Skip(ref context, out next);
		}
		
		private static void Dispose(ref ZipContext<U, T2, C2, T, C> context, out Option<U> next) {
			next = new Option<U>();
			context.bd.Release();
			context.left.dispose(ref context.left.context, out context.left.current);
			context.right.dispose(ref context.right.context, out context.right.current);
		}

		#endregion

	}
	
	#endregion
	
	#region With selector and parameter
	
	public struct ZipContext<U, T2, C2, T, C, P> {

		#region Slinqs
		
		public static Slinq<U, ZipContext<U, T2, C2, T, C, P>> Zip(Slinq<T, C> left, Slinq<T2, C2> right, Fn<T, T2, P, U> selector, P parameter, ZipRemoveFlags removeFlags) {
			return new Slinq<U, ZipContext<U, T2, C2, T, C, P>>(
				skip,
				remove,
				dispose,
				new ZipContext<U, T2, C2, T, C, P>(left, right, selector, parameter, removeFlags));
		}

		#endregion

		#region Context
		
		private bool needsMove;
		private Slinq<T, C> left;
		private Slinq<T2, C2> right;
		private readonly Fn<T, T2, P, U> selector;
		private readonly P parameter;
		private readonly ZipRemoveFlags removeFlags;
		
		#pragma warning disable 0414
		private BacktrackDetector bd;
		#pragma warning restore 0414

		private ZipContext(Slinq<T, C> left, Slinq<T2, C2> right, Fn<T, T2, P, U> selector, P parameter, ZipRemoveFlags removeFlags) {
			this.needsMove = false;
			this.left = left;
			this.right = right;
			this.selector = selector;
			this.parameter = parameter;
			this.removeFlags = removeFlags;
			
			this.bd = BacktrackDetector.Borrow();
		}
		
		#endregion
		
		#region Delegates
		
		private static readonly Mutator<U, ZipContext<U, T2, C2, T, C, P>> skip = Skip;
		private static readonly Mutator<U, ZipContext<U, T2, C2, T, C, P>> remove = Remove;
		private static readonly Mutator<U, ZipContext<U, T2, C2, T, C, P>> dispose = Dispose;

		private static void Skip(ref ZipContext<U, T2, C2, T, C, P> context, out Option<U> next) {
			context.bd.DetectBacktrack();
			
			if (context.needsMove) {
				context.left.skip(ref context.left.context, out context.left.current);
				context.right.skip(ref context.right.context, out context.right.current);
			} else {
				context.needsMove = true;
			}
			
			if (context.left.current.isSome && context.right.current.isSome) {
				next = new Option<U>(context.selector(context.left.current.get, context.right.current.get, context.parameter));
			} else {
				next = new Option<U>();
				context.bd.Release();
				if (context.left.current.isSome) {
					context.left.dispose(ref context.left.context, out context.left.current);
				} else if (context.right.current.isSome) {
					context.right.dispose(ref context.right.context, out context.right.current);
				}
			}
		}

		private static void Remove(ref ZipContext<U, T2, C2, T, C, P> context, out Option<U> next) {
			context.bd.DetectBacktrack();
			
			context.needsMove = false;
			
			if ((context.removeFlags & ZipRemoveFlags.Left) == ZipRemoveFlags.Left) {
				context.left.remove(ref context.left.context, out context.left.current);
			} else {
				context.left.skip(ref context.left.context, out context.left.current);
			}
			
			if ((context.removeFlags & ZipRemoveFlags.Right) == ZipRemoveFlags.Right) {
				context.right.remove(ref context.right.context, out context.right.current);
			} else {
				context.right.skip(ref context.right.context, out context.right.current);
			}

			Skip(ref context, out next);
		}

		private static void Dispose(ref ZipContext<U, T2, C2, T, C, P> context, out Option<U> next) {
			next = new Option<U>();
			context.bd.Release();
			context.left.dispose(ref context.left.context, out context.left.current);
			context.right.dispose(ref context.right.context, out context.right.current);
		}

		#endregion
		
	}
	
	#endregion
	
}
