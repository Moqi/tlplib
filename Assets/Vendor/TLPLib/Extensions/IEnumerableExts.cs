using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class IEnumerableExts {
    public static Option<B> findWithIndex<A, B>(
      this IEnumerable<A> enumerable, Fn<A, int, Option<B>> finder
    ) {
      var idx = 0;
      var list = enumerable as IList<A>;
      if (list != null) {
        for (idx = 0; idx < list.Count; idx++) {
          var opt = finder(list[idx], idx);
          if (opt.isDefined) return opt;
        }
        return F.none<B>();
      }

      var linkedList = enumerable as ILinkedList<A>;
      if (linkedList == null) {
        var mutLL = enumerable as LinkedList<A>;
        if (mutLL != null) linkedList = new ReadOnlyLinkedList<A>(mutLL);
      }
      if (linkedList != null) {
        var current = linkedList.First;
        while (current != null) {
          var opt = finder(current.Value, idx);
          if (opt.isDefined) return opt;
          current = current.Next;
          idx++;
        }
        return F.none<B>();
      }

      foreach (var a in enumerable) {
        var opt = finder(a, idx);
        if (opt.isDefined) return opt;
        idx++;
      }
      return F.none<B>();
    }

    public static Option<Tpl<A, int>> findWithIndex<A>(
      this IEnumerable<A> enumerable, Fn<A, int, bool> predicate
    ) {
      return enumerable.findWithIndex((a, i) =>
        predicate(a, i) ? F.some(F.t(a, i)) : F.none<Tpl<A, int>>()
      );
    }

    public static Option<Tpl<A, int>> findWithIndex<A>(
      this IEnumerable<A> enumerable, Fn<A, bool> predicate
    ) {
      return enumerable.findWithIndex((a, i) => predicate(a));
    }

    public static void each<A>(this IEnumerable<A> enumerable, Act<A> element) {
      enumerable.eachWithIndex((e, i) => element(e));
    }

    public static void eachWithIndex<A>(
      this IEnumerable<A> enumerable, Act<A, int> element
    ) {
      var none = F.none<A>();
      enumerable.findWithIndex((e, i) => {
        element(e, i);
        return none;
      });
    }

    public static bool exists<A>(
      this IEnumerable<A> enumerable, Fn<A, bool> predicate
    ) {
      return enumerable.findOpt(predicate).isDefined;
    }

    public static bool forall<A>(
      this IEnumerable<A> enumerable, Fn<A, bool> predicate
    ) {
      return enumerable.findOpt(a => ! predicate(a)).isEmpty;
    }

    public static Option<float> avg<A>(
      this IEnumerable<A> enumerable, Fn<A, float> extractor
    ) {
      return enumerable.reduceLeft(
        e => F.t(extractor(e), 1),
        (s, e) => F.t(s._1 + extractor(e), s._2 + 1)
      ).map(t => t._1 / t._2);
    }

    public static Option<int> sum<A>(
      this IEnumerable<A> enumerable, Fn<A, int> extractor
    ) {
      return enumerable.reduceLeft(extractor, (sum, e) => sum + extractor(e));
    }

    public static String asString(
      this IEnumerable enumerable, 
      bool newlines=true, bool fullClasses=false
    ) {
      var items = (
        from object item in enumerable
        let enumItem = item as IEnumerable
        select enumItem == null ? item.ToString() : enumItem.asString()
      ).ToArray();
      var itemsStr = 
        string.Join(string.Format(",{0} ", newlines ? "\n " : ""), items);
      if (items.Length != 0 && newlines) itemsStr = "\n  " + itemsStr + "\n";

      var type = enumerable.GetType();
      return string.Format(
        "{0}[{1}]",
        fullClasses ? type.FullName : type.Name,
        itemsStr
      );
    }

    public static string mkString<A>(
      this IEnumerable<A> enumerable, string sep, string start=null, string end=null
    ) {
      var b = new StringBuilder();
      b.Append(start ?? "");
      enumerable.eachWithIndex((elem, idx) => {
        if (idx == 0) b.Append(elem);
        else {
          b.Append(sep);
          b.Append(elem);
        }
      });
      b.Append(end ?? "");

      return b.ToString();
    }

    public static Option<A> headOpt<A>(this IEnumerable<A> enumerable) {
      return enumerable.findWithIndex((a, i) => F.some(a));
    }

    public static Option<A> lastOpt<A>(this IEnumerable<A> enumerable) {
      var list = enumerable as IList<A>;
      if (list != null)
        return list.Count == 0 ? F.none<A>() : F.some(list[list.Count - 1]);

      var linkedList = enumerable as LinkedList<A>;
      if (linkedList != null)
        return F.opt(linkedList.Last).map(_ => _.Value);

      return enumerable.findOrLast(a => false);
    }

    /**
     * Iterates the collection. Tries to find a member using predicate. If it
     * doesn't find one, returns last element of enumerable.
     * 
     * Returns None if enumerable is empty.
     **/
    public static Option<A> findOrLast<A>(
      this IEnumerable<A> enumerable, Fn<A, bool> predicate
    ) {
      var last = F.none<A>();
      return enumerable.findWithIndex((a, i) => {
        if (predicate(a)) return F.some(a);
        last = F.some(a);
        return F.none<A>();
      }).orElse(() => last);
    }

    public static B foldLeft<A, B>(
      this IEnumerable<A> enumerable, B initialState,
      Fn<B, A, B> folder
    ) {
      var state = initialState;
      enumerable.each(e => state = folder(state, e));
      return state;
    }

    public static Option<B> reduceLeft<A, B>(
      this IEnumerable<A> enumerable, Fn<A, B> initialStateExtractor, 
      Fn<B, A, B> folder
    ) {
      var state = F.none<B>();
      enumerable.each(e => state = state.fold(
        () => F.some(initialStateExtractor(e)),
        s => F.some(folder(s, e))
      ));
      return state;
    }

    public static Option<A> reduceLeft<A>(
      this IEnumerable<A> enumerable, Fn<A, A, A> folder
    ) { return enumerable.reduceLeft(_ => _, folder); }

    // AOT safe version of Min and Max.
    public static Option<A> minMax<A>(
      this IEnumerable<A> enumerable, Fn<A, A, bool> keepLeft
    ) {
      return enumerable.foldLeft(F.none<A>(), (currentOpt, a) =>
        currentOpt.map(c => keepLeft(c, a) ? c : a).orElse(F.some(a))
      );
    }

    public static Option<T> findOpt<T>(
      this IEnumerable<T> enumerable, Fn<T, bool> predicate
    ) {
      return enumerable.findWithIndex((e, i) => predicate(e)).map(t => t._1);
    }

    // Deprecated: use #findWithIndex instead.
    public static Option<B> findFlatMap<A, B>(
      this IEnumerable<A> enumerable, Fn<A, Option<B>> predicate
    ) {
      return enumerable.findWithIndex((a, i) => predicate(a));
    }
    
    public static Option<int> indexWhere<T>(
      this IEnumerable<T> enumerable, Fn<T, bool> predicate
    ) {
      return enumerable.findWithIndex(predicate).map(t => t._2);
    }

    /** Create enumerable with 1 element **/
    public static IEnumerable<T> Yield<T>(this T obj) {
      yield return obj;
    }

    public static IEnumerable<Tpl<A, B>> zip<A, B>(
      this IEnumerable<A> enum1, IEnumerable<B> enum2, bool strict=true
    ) {
      var e1 = enum1.GetEnumerator();
      var e2 = enum2.GetEnumerator();

      bool hasMore;
      do {
        var hasE1 = e1.MoveNext();
        // Stop iterating if we're not strict and first enumerable is finished.
        if (!strict && !hasE1) break;

        var hasE2 = e2.MoveNext();
        if (hasE1 != hasE2) throw new Exception(
          "Cannot zip through enumerables that are not the same size! " +
          string.Format("E1: {0}, E2: {1}", hasE1, hasE2)
        );

        if (hasE1) yield return F.t(e1.Current, e2.Current);
        hasMore = hasE1;
      } while (hasMore);
    }

    public static IEnumerable<Tpl<A, int>> zipWithIndex<A>(
      this IEnumerable<A> enumerable
    ) {
      var idx = -1;
      return enumerable.Select(v => {
        idx++;
        return F.t(v, idx);
      });
    }

    public static A randomElementByWeight<A>(
      this IEnumerable<A> sequence, 
      // If we change to Func here, Unity crashes. So fun.
      Fn<A, float> weightSelector
    ) {
      var totalWeight = sequence.Sum(i => weightSelector(i));
      // The weight we are after...
      var itemWeightIndex = (float) (new Random().NextDouble() * totalWeight);
      var currentWeightIndex = 0f;

      foreach (var item in sequence) {
        currentWeightIndex += weightSelector(item);
        // If we've hit or passed the weight we are after for this item then it's the one we want....
        if (currentWeightIndex >= itemWeightIndex) return item;
      }

      throw new IllegalStateException();
    }

    /**
     * Returns tuple of linked lists where first one contains all the items
     * that matched the predicate and second - those who didn't.
     **/
    public static Tpl<LinkedList<A>, LinkedList<A>> partition<A>(
      this IEnumerable<A> enumerable, Fn<A, bool> predicate
    ) {
      var trues = new LinkedList<A>();
      var falses = new LinkedList<A>();
      enumerable.each(a => {
        if (predicate(a)) trues.AddLast(a);
        else falses.AddLast(a);
      });
      return F.t(trues, falses);
    }
  }
}
