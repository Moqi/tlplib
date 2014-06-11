using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Iter {
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

    /* ILinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>> iter<A>(
      this ILinkedList<A> list, bool reverse = false
    ) {
      if (list.Count == 0) return Iter<A, Tpl<LinkedListNode<A>, int, bool>>.empty;

      return new Iter<A, Tpl<LinkedListNode<A>, int, bool>>(
        // list node, index, reverse
        F.t(
          reverse ? list.Last : list.First,
          reverse ? list.Count - 1 : 0, reverse
        ),
        ctx => {
          var newNode = ctx._3 ? ctx._1.Previous : ctx._1.Next;
          return newNode == null
            ? F.none<Tpl<LinkedListNode<A>, int, bool>>()
            : F.some(F.t(newNode, ctx._2 + (ctx._3 ? -1 : 1), ctx._3));
        },
        ctx => ctx._1.Value,
        ctx => F.some(elementsLeft(ctx._1.List.Count, ctx._2, ctx._3))
      );
    }

    /* Reverse ILinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>>
    rIter<A>(this ILinkedList<A> list) { return list.iter(true); }

    /* LinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>> iter<A>(
      this LinkedList<A> list, bool reverse = false
    ) { return ReadOnlyLinkedList.a(list).iter(reverse); }

    /* Reverse LinkedList to Iter wrapper. */
    public static Iter<A, Tpl<LinkedListNode<A>, int, bool>> 
    rIter<A>(this LinkedList<A> list) { return list.iter(true); }

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
