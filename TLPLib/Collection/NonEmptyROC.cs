using System.Collections.Generic;
using System.Collections.ObjectModel;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Collection {
  public static class NonEmptyROC {
    public static NonEmptyROC<A> a<A>(A first, params A[] rest) 
    { return NonEmptyROC<A>.a(first, rest); }

    public static Option<NonEmptyROC<A>> a<A>(IList<A> list) 
    { return NonEmptyROC<A>.a(list); }
  }

  /* Non-empty Read-Only Collection */
  public class NonEmptyROC<A> : ReadOnlyCollection<A> {
    private NonEmptyROC(IList<A> list) : base(list) { }

    public static NonEmptyROC<A> a(A first, params A[] rest) {
      var arr = new A[rest.Length + 1];
      arr[0] = first;
      rest.CopyTo(arr, 1);
      return new NonEmptyROC<A>(arr);
    }

    public static Option<NonEmptyROC<A>> a(IList<A> list) 
    { return (!list.isEmpty()).opt(new NonEmptyROC<A>(list)); }
  }
}
