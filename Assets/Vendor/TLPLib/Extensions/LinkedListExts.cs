using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Collection;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class LinkedListExts {
    public static bool isEmpty<A>(this ILinkedList<A> list) 
    { return list.Count == 0; }

    public static bool isEmpty<A>(this LinkedList<A> list) 
    { return list.Count == 0; }
  }
}
