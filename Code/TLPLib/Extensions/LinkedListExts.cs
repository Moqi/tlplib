using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Collection;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class LinkedListExts {
    public static LinkedList<B> map<A, B>(this ILinkedList<A> list, Fn<A, B> mapper) {
      var nList = new LinkedList<B>();
      list.each(e => nList.AddLast(mapper(e)));
      return nList;
    }

    public static LinkedList<B> mapWithIndex<A, B>(
      this ILinkedList<A> list, Fn<A, int, B> mapper
    ) {
      var nList = new LinkedList<B>();
      list.eachWithIndex((e, i) => nList.AddLast(mapper(e, i)));
      return nList;
    }

    public static LinkedList<A> filter<A>(
      this LinkedList<A> list, Fn<A, bool> predicate
    ) {
      var nList = new LinkedList<A>();
      list.each(e => { if (predicate(e)) nList.AddLast(e); });
      return nList;
    }
  }
}
