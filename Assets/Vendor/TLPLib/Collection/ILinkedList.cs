using System.Collections.Generic;

namespace com.tinylabproductions.TLPLib.Collection {
  /**
   * Read only interface to linked list.
   **/

  public interface ILinkedList<A> : IEnumerable<A> {
    int Count { get; }
    bool Contains(A item);
    void CopyTo(A[] array, int arrayIndex);
    LinkedListNode<A> First { get; }
    LinkedListNode<A> Last { get; }
    LinkedListNode<A> Find(A value);
    LinkedListNode<A> FindLast(A value);
  }
}
