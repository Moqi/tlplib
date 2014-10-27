using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class LinkedListExts {
    public static bool isEmpty<A>(this ReadOnlyLinkedList<A> list) 
    { return list.Count == 0; }

    public static bool isEmpty<A>(this LinkedList<A> list) 
    { return list.Count == 0; }

    /* Removes first element and returns it. */
    public static Option<A> shift<A>(this LinkedList<A> list) {
      return F.opt(list.First).map(n => {
        list.RemoveFirst();
        return n.Value;
      });
    }

    /* Removes last element and returns it. */
    public static Option<A> pop<A>(this LinkedList<A> list) {
      return F.opt(list.Last).map(n => {
        list.RemoveLast();
        return n.Value;
      });
    }

    /* As foreach, but does not freak out if underlying list is changed. */
    public static void iterateWhileChangedN<A>(this LinkedList<A> list, Act<LinkedListNode<A>> elem) {
      var node = list.First;
      while (node != null) {
        elem(node);
        node = node.Next;
      }
    }

    /* As foreach, but does not freak out if underlying list is changed. */
    public static void iterateWhileChanged<A>(this LinkedList<A> list, Act<A> elem) {
      list.iterateWhileChangedN(node => elem(node.Value));
    }

    public static void removeWhere<A>(this LinkedList<A> list, Fn<A, bool> predicate) {
      list.iterateWhileChangedN(node => {
        if (predicate(node.Value)) list.Remove(node);
      });
    }
  }
}
