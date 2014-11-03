using System.Collections.Generic;
using System.Collections.ObjectModel;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Collection {
  public static class NonEmptyROC {
    public static Option<NonEmptyROC<A>> a<A>(IList<A> list) 
    { return NonEmptyROC<A>.a(list); }
  }

  /* Non-empty Read-Only Collection */
  public class NonEmptyROC<A> : ReadOnlyCollection<A> {
    private NonEmptyROC(IList<A> list) : base(list) { }

    public static Option<NonEmptyROC<A>> a(IList<A> list) 
    { return (!list.isEmpty()).opt(new NonEmptyROC<A>(list)); }
  }
}
