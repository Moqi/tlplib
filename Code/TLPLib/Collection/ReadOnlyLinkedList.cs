using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace com.tinylabproductions.TLPLib.Collection {
  [ComVisible(false)]
  [DebuggerDisplay("Count = {Count}")]
  class ReadOnlyLinkedList<A> : ILinkedList<A> {
    private readonly LinkedList<A> backing;

    public ReadOnlyLinkedList(LinkedList<A> backingList) {
      backing = backingList;
    }

    public IEnumerator<A> GetEnumerator() { return backing.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    public bool Contains(A item) { return backing.Contains(item); }

    public void CopyTo(A[] array, int arrayIndex) {
      backing.CopyTo(array, arrayIndex);
    }

    public int Count { get { return backing.Count; } }

    public LinkedListNode<A> First { get { return backing.First; } }
    public LinkedListNode<A> Last { get { return backing.Last; } }
    public LinkedListNode<A> Find(A value) { return backing.Find(value); }
    public LinkedListNode<A> FindLast(A value) 
      { return backing.FindLast(value); }
  }
}
