using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Collection {
//  public class UnorderedArray<A> : ICollection<A> {
//    private A[] backing;
//    private int count = 0;
//
//    public UnorderedArray(int initialCapacity=4) {
//      backing = new A[initialCapacity];
//    }
//
//    public IEnumerator<A> GetEnumerator() {
//      throw new NotImplementedException();
//    }
//
//    IEnumerator IEnumerable.GetEnumerator() {
//      return GetEnumerator();
//    }
//
//    public void Add(A item) {
//      if (count == backing.Length) growTo(count * 2);
//    }
//
//    public void Clear() {
//      for (var idx = 0; idx < backing.Length; idx++) backing[idx] = default(A);
//      count = 0;
//    }
//
//    public bool Contains(A item) {
//      return backing.findOpt(_ => _ == item).isDefined;
//    }
//
//    public void CopyTo(A[] array, int arrayIndex) {
//      throw new NotImplementedException();
//    }
//
//    public bool Remove(A item) {
//      backing.findWithIndex(a => a == item).fold(
//        () => false,
//
//      )
//    }
//
//    public int Count { get { return count; } }
//    public bool IsReadOnly { get { return false; } }
//
//    private void growTo(int size) {
//      
//    }
//  }
}
