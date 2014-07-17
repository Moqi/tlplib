using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Iter {
// ReSharper disable RedundantTypeArgumentsOfMethod
  /**
   * Helper methods for alocating Iters.
   **/
  public static class Iter {
    /* Returns Iter that goes from `from` to `to` by `step`.
     * Both `from` and `to` are inclusive. If `step` is 0 this will
     * cause in an endless sequence of `from`. */
    public static Iter<int> range(
      int from, int to, int step = 1
    ) {
      var cache = Iter<int>.fnCache(".range");
      if ((step > 0 && from > to) || (step < 0 && to > from))
        return cache.empty;

      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<int, int, int>) ictx);
          var _current = ctx._1; var _end = ctx._2; var _step = ctx._3;
          var newCurrent = _current + _step;
          return
            ((_step > 0 && newCurrent > _end) || (_step < 0 && newCurrent < _end))
            ? F.none<ICtx>()
            : F.some(Ctx.a(newCurrent, _end, _step));
        },
        ictx => ((Ctx<int, int, int>) ictx)._1,
        ictx => {
          var ctx = ((Ctx<int, int, int>) ictx);
          var _current = ctx._1; var _end = ctx._2; var _step = ctx._3;
          return _step == 0 ? F.none<int>() :
            F.some(Math.Abs(_end - _current) / Math.Abs(_step) + 1);
        }
      ))).iter(Ctx.a(from, to, step));
    }
  }

  /**
   * Extension methods for alocating Iters.
   * 
   * Following versions are defined:
   * 
   * * .iter() - standart front to back iterator.
   * * .rIter() - reverse iterator for collections that expose .Count.
   * * .hIter() - Iter which needs a heap alocation.
   * * .singleIter() - Iter which will only emit one element.
   **/
  public static class IterExts {
    #region IList

    /* IList to Iter wrapper. */
    public static Iter<A> iter<A>(
      this IList<A> list, bool reverse = false
    ) {
      var cache = Iter<A>.fnCache(".iter(IList)");

      if (list.Count == 0) return cache.empty;
      return (cache.fns ?? (cache.fns = cache.build(
        // list, index, reverse
        ictx => {
          var ctx = ((Ctx<IList<A>, int, bool>) ictx);
          var lst = ctx._1; var idx = ctx._2; var rev = ctx._3;
          var newIndex = idx + (rev ? -1 : 1);
          return newIndex < 0 || newIndex >= lst.Count
            ? F.none<ICtx>()
            : F.some(Ctx.a(lst, newIndex, rev));
        },
        ictx => {
          var ctx = ((Ctx<IList<A>, int, bool>) ictx);
          return ctx._1[ctx._2];
        },
        ictx => {
          var ctx = ((Ctx<IList<A>, int, bool>) ictx);
          return F.some(elementsLeft(ctx._1.Count, ctx._2, ctx._3));
        }
      ))).iter(Ctx.a(list, reverse ? list.Count - 1 : 0, reverse));
    }

    /* Reverse IList to Iter wrapper. */
    public static Iter<A> rIter<A>(this IList<A> list) 
    { return list.iter(true); }

    #endregion

    #region LinkedList
    
    /* LinkedList like to Iter wrapper. */
    private static Iter<A> llIter<A>(int elements, ICtx context) {
      var cache = Iter<A>.fnCache(".-llIter");

      if (elements == 0) return cache.empty;
      return (cache.fns ?? (cache.fns = cache.build(
        // list node, index, reverse
        ictx => {
          var ctx = ((Ctx<LinkedListNode<A>, int, bool>) ictx);
          var node = ctx._1; var index = ctx._2; var rev = ctx._3;
          var newNode = rev ? node.Previous : node.Next;
          return newNode == null
            ? F.none<ICtx>()
            : F.some(Ctx.a(newNode, index + (rev ? -1 : 1), rev));
        }, 
        ictx => ((Ctx<LinkedListNode<A>, int, bool>) ictx)._1.Value,
        ictx => {
          var ctx = ((Ctx<LinkedListNode<A>, int, bool>) ictx);
          return F.some(elementsLeft(ctx._1.List.Count, ctx._2, ctx._3));
        }
      ))).iter(context);
    }

    /* LinkedList to Iter wrapper. */
    public static Iter<A> iter<A>(
      this LinkedList<A> list, bool reverse = false
    ) {
      return llIter<A>(list.Count, Ctx.a(
        // list node, index, reverse
        reverse ? list.Last : list.First, reverse ? list.Count - 1 : 0, reverse
      ));
    }

    /* Reverse LinkedList to Iter wrapper. */
    public static Iter<A> rIter<A>(this LinkedList<A> list) 
    { return list.iter(true); }

    /* ReadOnlyLinkedList to Iter wrapper. */
    public static Iter<A> iter<A>(
      this ReadOnlyLinkedList<A> list, bool reverse = false
    ) {
      return llIter<A>(list.Count, Ctx.a(
        // list node, index, reverse
        reverse ? list.Last : list.First, reverse ? list.Count - 1 : 0, reverse
      ));
    }

    /* Reverse ReadOnlyLinkedList to Iter wrapper. */
    public static Iter<A> rIter<A>(this ReadOnlyLinkedList<A> list) 
    { return list.iter(true); }

    #endregion

    #region IEnumerable

    /* IEnumerable to Iter wrapper. This is prefixed with h, because it 
     * allocates heap objects. */
    public static Iter<A> hIter<A>(
      this IEnumerable<A> enumerable
    ) {
      var cache = Iter<A>.fnCache(".hIter");
      var enumerator = enumerable.GetEnumerator();
      if (!enumerator.MoveNext()) return cache.empty;

      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var e = ((Ctx<IEnumerator<A>, Unit, Unit>) ictx)._1;
          if (e.MoveNext()) return F.some(Ctx.a(e));
          else {
            e.Dispose();
            return F.none<ICtx>();
          }
        },
        ictx => ((Ctx<IEnumerator<A>, Unit, Unit>) ictx)._1.Current,
        _ => F.none<int>()
      ))).iter(Ctx.a(enumerator));
    }

    #endregion

    #region Single Element

    /* Returns Iter that emits one element. */
    public static Iter<A> singleIter<A>(this A any) {
      var cache = Iter<A>.fnCache(".singleIter");
      return (cache.fns ?? (cache.fns = cache.build(
        _ => F.none<ICtx>(), 
        ictx => ((Ctx<A, Unit, Unit>) ictx)._1, 
        _ => F.some(1)
      ))).iter(Ctx.a(any));
    }

    #endregion

    #region Private methods

    private static int elementsLeft(int total, int currentIdx, bool reverse) {
      return reverse ? currentIdx + 1 : total - currentIdx;
    }

    #endregion
  }
// ReSharper restore RedundantTypeArgumentsOfMethod
}