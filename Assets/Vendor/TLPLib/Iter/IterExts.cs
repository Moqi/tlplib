using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Iter {
  /**
   * Helper methods for alocating Iters.
   **/
  public static class Iter {
    /* Returns Iter that goes from `from` to `to` by `step`.
     * Both `from` and `to` are inclusive. If `step` is 0 this will
     * cause in an endless sequence of `from`. */
    public static Iter<int, Tpl<int, int, int>> range(
      int from, int to, int step = 1
    ) {
      if ((step > 0 && from > to) || (step < 0 && to > from))
        return Iter<int, Tpl<int, int, int>>.empty;

      return new Iter<int, Tpl<int, int, int>>(
        F.t(from, to, step),
        ctx => ctx.ua((_current, _end, _step) => {
          var newCurrent = _current + _step;
          return
            ((_step > 0 && newCurrent > _end) || (_step < 0 && newCurrent < _end))
            ? F.none<Tpl<int, int, int>>()
            : F.some(F.t(newCurrent, _end, _step));
        }),
        ctx => ctx._1,
        ctx => ctx.ua((_current, _end, _step) => 
          _step == 0 ? F.none<int>() :
          F.some(Math.Abs(_end - _current) / Math.Abs(_step) + 1)
        )
      );
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
    public static Iter<A, Tpl<IList<A>, int, bool>> iter<A>(
      this IList<A> list, bool reverse = false
    ) {
      if (list.Count == 0) return Iter<A, Tpl<IList<A>, int, bool>>.empty;

      return new Iter<A, Tpl<IList<A>, int, bool>>(
        // list, index, reverse
        F.t(list, reverse ? list.Count - 1 : 0, reverse),
        ctx => { var lst = ctx._1; var idx = ctx._2; var rev = ctx._3;
          var newIndex = idx + (rev ? -1 : 1);
          return newIndex < 0 || newIndex >= lst.Count
            ? F.none<Tpl<IList<A>, int, bool>>()
            : F.some(F.t(lst, newIndex, rev));
        },
        ctx => ctx._1[ctx._2],
        ctx => F.some(elementsLeft(ctx._1.Count, ctx._2, ctx._3))
      );
    }

    /* Reverse IList to Iter wrapper. */
    public static Iter<A, Tpl<IList<A>, int, bool>> 
    rIter<A>(this IList<A> list) { return list.iter(true); }

    #endregion

    #region LinkedList

    private static Option<Tpl<LinkedListNode<A>, int, bool>> linkedListLikeSkipper<A>(
      Tpl<LinkedListNode<A>, int, bool> ctx
    ) {
      var node = ctx._1; var index = ctx._2; var rev = ctx._3;
      var newNode = rev ? node.Previous : node.Next;
      return newNode == null
        ? F.none<Tpl<LinkedListNode<A>, int, bool>>()
        : F.some(F.t(newNode, index + (rev ? -1 : 1), rev));
    }

    private static A linkedListLikeGetter<A>(Tpl<LinkedListNode<A>, int, bool> ctx) {
      return ctx._1.Value;
    }

    private static Option<int> linkedListLikeSizeHint<A>(Tpl<LinkedListNode<A>, int, bool> ctx) {
      return F.some(elementsLeft(ctx._1.List.Count, ctx._2, ctx._3));
    }

    /* LinkedList like to Iter wrapper. */
    private static Iter<A, Tpl<LinkedListNode<A>, int, bool>> iter<A>(
      int elements, Tpl<LinkedListNode<A>, int, bool> context
    ) {
      if (elements == 0) return Iter<A, Tpl<LinkedListNode<A>, int, bool>>.empty;

      return new Iter<A, Tpl<LinkedListNode<A>, int, bool>>(
        // list node, index, reverse
        context, linkedListLikeSkipper, linkedListLikeGetter, linkedListLikeSizeHint
      );
    }

    /* LinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>> iter<A>(
      this LinkedList<A> list, bool reverse = false
    ) {
      return iter(list.Count, F.t(
        // list node, index, reverse
        reverse ? list.Last : list.First, reverse ? list.Count - 1 : 0, reverse
      ));
    }

    /* Reverse LinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>>
    rIter<A>(this LinkedList<A> list) { return list.iter(true); }

    /* ReadOnlyLinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>> iter<A>(
      this ReadOnlyLinkedList<A> list, bool reverse = false
    ) {
      return iter(list.Count, F.t(
        // list node, index, reverse
        reverse ? list.Last : list.First, reverse ? list.Count - 1 : 0, reverse
      ));
    }

    /* Reverse ReadOnlyLinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>>
    rIter<A>(this ReadOnlyLinkedList<A> list) { return list.iter(true); }

    #endregion

    #region IEnumerable

    /* IEnumerable to Iter wrapper. This is prefixed with h, because it 
     * allocates heap objects. */
    public static Iter<A, IEnumerator<A>> hIter<A>(
      this IEnumerable<A> enumerable
    ) {
      var enumerator = enumerable.GetEnumerator();
      if (!enumerator.MoveNext()) return Iter<A, IEnumerator<A>>.empty;

      return new Iter<A, IEnumerator<A>>(
        enumerator,
        e => {
          if (e.MoveNext()) return F.some(e);
          else {
            e.Dispose();
            return F.none<IEnumerator<A>>();
          }
        },
        e => e.Current,
        e => F.none<int>()
      );
    }

    #endregion

    #region Single Element

    /* Returns Iter that emits one element. */
    public static Iter<A, A> singleIter<A>(this A any) {
      return new Iter<A, A>(any, _ => F.none<A>(), _ => _, _ => F.some(1));
    }

    #endregion

    #region Private methods

    private static int elementsLeft(int total, int currentIdx, bool reverse) {
      return reverse ? currentIdx + 1 : total - currentIdx;
    }

    #endregion
  }
}
