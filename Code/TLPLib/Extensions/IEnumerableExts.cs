using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class IEnumerableExts {
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

    public static string mkString(
      this IEnumerable enumerable, string sep, string start=null, string end=null
    ) {
      var b = new StringBuilder();
      var first = true;
      b.Append(start ?? "");
      foreach (var x in enumerable) {
        if (first) {
          b.Append(x);
          first = false;
        }
        else {
          b.Append(sep);
          b.Append(x);
        }
      }
      b.Append(end ?? "");

      return b.ToString();
    }

    public static Option<Tpl<T, int>> FindWithIndex<T>(
      this IEnumerable<T> enumerable, Fn<T, bool> predicate
    ) {
      var index = 0;
      foreach (var item in enumerable) {
        if (predicate(item)) return F.some(F.t(item, index));
        index += 1;
      }
      return F.none<Tpl<T, int>>();
    }

    public static Option<A> headOpt<A>(this IEnumerable<A> enumerable) {
      foreach (var e in enumerable) return F.some(e);
      return F.none<A>();
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
      foreach (var a in enumerable) {
        var current = F.some(a);
        if (predicate(a)) return current;
        last = current;
      }
      return last;
    }

    private static Option<A> minMax<A, B>(
      this IEnumerable<A> enumerable, Fn<A, B> selector, Fn<int, bool> decider
    ) {
      var aOpt = F.none<A>();
      var bOpt = F.none<B>();

      var comparer = Comparer<B>.Default;
      foreach (var a in enumerable) {
        var b = selector(a);
        if (
          // ReSharper disable once RedundantTypeArgumentsOfMethod
          // Mono Compiler bug.
          bOpt.fold<bool>(() => true, prevB => decider(comparer.Compare(b, prevB)))
        ) {
          aOpt = F.some(a);
          bOpt = F.some(b);
        }
      }

      return aOpt;
    }

    public static Option<A> min<A, B>(
      this IEnumerable<A> enumerable, Fn<A, B> selector
    ) {
      return enumerable.minMax(selector, _ => _ < 0);
    }

    public static Option<A> max<A, B>(
      this IEnumerable<A> enumerable, Fn<A, B> selector
    ) {
      return enumerable.minMax(selector, _ => _ > 0);
    }

    public static Option<T> FindOpt<T>(
      this IEnumerable<T> enumerable, Fn<T, bool> predicate
    ) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug.
      return enumerable.FindWithIndex(predicate).map<T>(t => t._1);
    }

    public static Option<B> FindFlatMap<A, B>(
      this IEnumerable<A> enumerable, Fn<A, Option<B>> predicate
    ) {
      foreach (var item in enumerable) {
        var opt = predicate(item);
        if (opt.isDefined) return opt;
      }
      return F.none<B>();
    }

    public static Option<int> IndexWhere<T>(
      this IEnumerable<T> enumerable, Fn<T, bool> predicate
    ) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug.
      return enumerable.FindWithIndex(predicate).map<int>(t => t._2);
    }

    /** Create enumerable with 1 element **/
    public static IEnumerable<T> Yield<T>(this T obj) {
      yield return obj;
    }

    public static IEnumerable<Tpl<A, B>> Zip<A, B>(
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

    public static IEnumerable<Tpl<A, int>> ZipWithIndex<A>(
      this IEnumerable<A> enumerable
    ) {
      var idx = -1;
      return enumerable.Select(v => {
        idx++;
        return F.t(v, idx);
      });
    }

    public static A RandomElementByWeight<A>(
      this IEnumerable<A> sequence, Func<A, float> weightSelector
    ) {
      var totalWeight = sequence.Sum(weightSelector);
      // The weight we are after...
      var itemWeightIndex = (float) (new Random().NextDouble() * totalWeight);
      var currentWeightIndex = 0f;

      foreach (
        var item in 
          from weightedItem in sequence 
          select new { Value = weightedItem, Weight = weightSelector(weightedItem) }
      ) {
        currentWeightIndex += item.Weight;

        // If we've hit or passed the weight we are after for this item then it's the one we want....
        if (currentWeightIndex >= itemWeightIndex)
          return item.Value;
      }

      throw new IllegalStateException();
    }

    public static void each<A>(this IEnumerable<A> enumerable, Act<A> action) {
      foreach (var a in enumerable) action(a);
    }

    public static void eachWithIndex<A>
    (this IEnumerable<A> enumerable, Act<A, uint> action) {
      var index = 0u;
      foreach (var a in enumerable) {
        action(a, index);
        index++;
      }
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
      foreach (var a in enumerable) {
        if (predicate(a)) trues.AddLast(a);
        else falses.AddLast(a);
      }
      return F.t(trues, falses);
    }
  }
}
