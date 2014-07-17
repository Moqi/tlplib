using System;
using System.Collections.Generic;
using System.Text;
using com.tinylabproductions.TLPLib.Functional;
using Smooth.Collections;
using Random = UnityEngine.Random;

namespace com.tinylabproductions.TLPLib.Iter {
  /**
   * Heap allocation free enumerator.
   * 
   * Basic usage:
   * 
   * for (
   *   var i = list.iter(); // obtain an Iter
   *   i;                   // check if is valid
   *   i++                  // advance to next value
   * ) {
   *   var current = ~i; // Fetch current value.
   * }
   * 
   * Or alternatively you can use one of the methods defined on Iter.
   * 
   * Adding new operations:
   * 
   * Obviously check out existing implementations. One thing to be noted,
   * an Iter must always be returned initialized to first position. This is
   * contrary to Enumerator, which starts off uninitialized. In other words,
   * .current from newly returned Iter must always have a value if it is 
   * possible.
   * 
   * Credits:
   * 
   * Iter is written by Artūras Šlajus (arturaz, http://arturaz.net).
   * 
   * Iter is based on Smooth Foundations library 
   * (https://github.com/pdo400/smooth.foundations) written by 
   * Patrick Whitesell (pdo400).
   * 
   * It is an amazing piece of software, however I felt like the ability to 
   * remove items was making it too complicated to write new operations. So
   * this is my take on this problem.
   **/
  public struct Iter<A> {
// ReSharper disable RedundantTypeArgumentsOfMethod
    #region Cache

    /* Function cache storage. */
    private static readonly IDictionary<string, FnCache> _fnCache = 
      new Dictionary<string, FnCache>();

    /* Several methods might use same Ctx signature, so we need to pass a diferentiator. */
    public static FnCache fnCache(string id) {
      FnCache cache;
      if (! _fnCache.TryGetValue(id, out cache)) {
        cache = new FnCache();
        _fnCache[id] = cache;
      }
      return cache;
    }

    /* Actual function cache that helps us cache fns and reduce boilerplate. */
    public class FnCache {
      public Fns fns;

      // ReSharper disable once MemberHidesStaticFromOuterClass
      public Iter<A> empty { get { return Iter<A>.empty; } }

      public Fns build(
        Fn<ICtx, Option<ICtx>> skipper, Fn<ICtx, A> getter, 
        Fn<ICtx, Option<int>> sizeHint
      ) { return new Fns(skipper, getter, sizeHint); }
    }

    /* Holder for Iter function cache. */
    public class Fns {
      public readonly Fn<ICtx, Option<ICtx>> skipper;
      public readonly Fn<ICtx, A> getter;
      public readonly Fn<ICtx, Option<int>> sizeHint;

      public Fns(Fn<ICtx, Option<ICtx>> skipper, Fn<ICtx, A> getter, Fn<ICtx, Option<int>> sizeHint) {
        this.skipper = skipper;
        this.getter = getter;
        this.sizeHint = sizeHint;
      }

      public Iter<A> iter(ICtx ctx) { return new Iter<A>(ctx, this); }
    }

    #endregion

    #region Internal state

    /* Takes a context and provides next context, which is None if Iter is 
     * done. */
    private readonly Fn<ICtx, Option<ICtx>> skipper;
    /* Extracts a value from context. */
    private readonly Fn<ICtx, A> getter;
    /* How many elements are left in this Iter? */
    private readonly Fn<ICtx, Option<int>> sizeHint;

    /* None if Iter is finished, Some if it's valid. */
    private Option<ICtx> state;

    #endregion

    #region Base interface

    public static readonly Iter<A> empty = new Iter<A>(F.unit);

    /* Empty constructor to prevent iOS issues. */
    private Iter(Unit empty) {
      state = F.none<ICtx>();
      skipper = null;
      getter = null;
      sizeHint = null;
    }

    public Iter(ICtx ctx, Fns fns) {
      state = F.some(ctx);
      skipper = fns.skipper;
      getter = fns.getter;
      sizeHint = fns.sizeHint;
    }

    public override string ToString() {
      return string.Format("Iter({0}|sh:{1})", state, elementsLeft);
    }

    /* Mutates Iter and moves it to next place in the sequence. */
    public void progress() {
      var oldState = state;
      state = state.flatMap(skipper);
      if (oldState.isDefined && state.isEmpty) oldState.get.release();
    }

    /* Does this Iter currently has a value? */
    public bool hasValue { get { return state.isDefined; } }

    /* Get the current value of Iter. Returns None if iter is finished. */
    public Option<A> current { get { return state.map(getter); } }

    /* Unsafe get the current value of Iter. 
     * Throws exception if iter is finished. */
    public A get { get {
      return current.getOrThrow(() => new IllegalStateException(string.Format(
        "Iter({0}, {1}) is finished.", typeof(A), typeof(Ctx))
      ));
    } }

    /* Skips Iter and returns new current element. */
    public Option<A> next { get {
      progress();
      return current;
    } }

    /* Returns how many elements are left in this Iter. 
     * Not always possible to know that (e.g. if iterating IEnumerable). */
    public Option<int> elementsLeft { get {
      return state.isEmpty ? F.some(0) : sizeHint(state.get);
    } }

    /* Alias to .hasValue */
    public static implicit operator bool(Iter<A> iter) { return iter.hasValue; }

    /* Alias to .get */
    public static A operator ~(Iter<A> iter) { return iter.get; }

    /* Alias to .skip() */
    public static Iter<A> operator ++(Iter<A> iter) {
      iter.progress();
      return iter;
    }

    #endregion
  }

  public static class IterOps {
    
    #region Conversions

    /* Iterates underlying sequence and returns a list from that. */
    public static List<A> toList<A>(this Iter<A> iter) {
      var elementsLeft = iter.elementsLeft;
      // No fold here, because of AOT...
      var list = 
        elementsLeft.isDefined ? F.emptyList<A>(elementsLeft.get) : new List<A>();
      for (var i = iter; i; i++) list.Add(~i);
      return list;
    }

    /* Same as toList() but returns an interface type. */
    public static IList<A> toIList<A>(this Iter<A> iter) { return iter.toList(); }

    /* Iterates underlying sequence and returns a linked list from that. */
    public static LinkedList<A> toLinkedList<A>(this Iter<A> iter) {
      var ll = new LinkedList<A>();
      for (var i = iter; i; i++) ll.AddLast(~i);
      return ll;
    }

    /* Iterates underlying sequence and returns an array from that.
     * 
     * Beware that this will cause in a temporary list for sources that do not
     * provide a size hint. */
    public static A[] toArray<A>(this Iter<A> iter) {
      var leftOpt = iter.elementsLeft;
      if (leftOpt.isDefined) {
        var arr = new A[leftOpt.get];
        var idx = 0;
        for (var i = iter; i; i++, idx++) arr[idx] = ~i;
        return arr;
      }
      else {
        var list = new List<A>();
        for (var i = iter; i; i++) list.Add(~i);
        return list.ToArray();
      }
    }

    #endregion

    #region Folds

    /* Folds this Iter to a value using given initial state and folder. */
    public static B foldLeft<A, B>(this Iter<A> iter, B initial, Fn<B, A, B> folder) {
      var current = initial;
      for (var i = iter; i; i++) current = folder(current, ~i);
      return current;
    }

    /* Reduces this Iter to a value using first element as initial 
     * state and given reducer.
     * 
     * Returns None if Iter is empty. */
    public static Option<A> reduceLeft<A>(this Iter<A> iter, Fn<A, A, A> reducer) {
      return iter.reduceLeft(_ => _, reducer);
    }

    /* Reduces this Iter to a value using first element passed through 
     * initial state extractor as initial state and given reducer.
     * 
     * Returns None if Iter is empty. */
    public static Option<B> reduceLeft<A, B>(this Iter<A> iter, 
      Fn<A, B> initialStateExtractor, Fn<B, A, B> reducer
    ) {
      if (iter) {
        var i = iter;
        var initial = ~(i++);
        return F.some(i.foldLeft(initialStateExtractor(initial), reducer));
      }
      else return F.none<B>();
    }

    /* Reduces this Iter to a value using a predicate that tells us whether
     * we should keep left argument.
     * 
     * Returns None if Iter is empty. */
    public static Option<A> keepLeft<A>(this Iter<A> iter, Fn<A, A, bool> keepLeft) {
      return iter.reduceLeft((a, b) => keepLeft(a, b) ? a : b);
    }

    /**
     * Returns tuple of linked lists where first one contains all the items
     * that matched the predicate and second - those who didn't.
     **/
    public static Tpl<LinkedList<A>, LinkedList<A>> partition<A>(
      this Iter<A> iter, Fn<A, bool> predicate
    ) {
      var trues = new LinkedList<A>();
      var falses = new LinkedList<A>();
      for (var i = iter; i; i++) {
        var a = ~i;
        if (predicate(a)) trues.AddLast(a);
        else falses.AddLast(a);
      }
      return F.t(trues, falses);
    }

    /* Iterates through sequence, extracts a float and returns average. 
     * Returns None if Iter is empty. */
    public static Option<float> avg<A>(this Iter<A> iter, Fn<A, float> extractor) {
      if (! iter) return F.none<float>();

      var total = 0f;
      var count = 0;

      for (var i = iter; i; i++) {
        total += extractor(~i);
        count += 1;
      }

      return F.some(total / count);
    }

    /* Constructs a string from this Iter. */
    public static string mkString<A>(
      this Iter<A> iter, 
      string separator, string start = null, string end = null
    ) {
      var b = new StringBuilder();
      if (start != null) b.Append(start);
      var idx = 0;
      for (var i = iter; i; i++, idx++) {
        if (idx == 0) b.Append(~i);
        else {
          b.Append(separator);
          b.Append(~i);
        }
      }
      if (end != null) b.Append(end);

      return b.ToString();
    }

    #endregion

    #region Traversals

    /* Lambda style for each. */
    public static void each<A>(this Iter<A> iter, Act<A> f) 
    { for (var i = iter; i; i++) f(~i); }

    /* Lambda style for each with index. */
    public static void eachWithIndex<A>(this Iter<A> iter, Act<A, int> f) {
      var idx = 0;
      for (var i = iter; i; i++, idx++) f(~i, idx);
    }

    /* Filters Iter with given predicate. */
    public static Iter<A> filter<A>(this Iter<A> iter, Fn<A, bool> __predicate) {
      // Find first item which satisfies given predicate.
      for (; iter && ! __predicate(~iter); iter++) {}

      var cache = Iter<A>.fnCache("#filter");
      // If we're done with the iterator, return empty Iter.
      if (! iter) return cache.empty;

      // Otherwise return new iterator that filters elements.
      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<Iter<A>, Fn<A, bool>, Unit>) ictx);
          var i = ctx._1; var p = ctx._2;
          // Skip to first next one where predicate is satisfied.
          while ((++i).hasValue && ! p(~i)) {}

          return i ? F.some(Ctx.a(i, p)) : F.none<ICtx>();
        },
        ictx => ((Ctx<Iter<A>, Fn<A, bool>, Unit>) ictx)._1.get,
        ctx => F.none<int>()
      ))).iter(Ctx.a(iter, __predicate));
    }

    /* Maps type A to type B. */
    public static Iter<B> map<A, B>(
      this Iter<A> iter, Fn<A, B> __mapper
    ) {
      var cache = Iter<B>.fnCache("#map");
      if (! iter) return cache.empty;

      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<Iter<A>, Fn<A, B>, Unit>) ictx);
          var i = ctx._1; var map = ctx._2;
          return (++i).hasValue ? F.some(Ctx.a(i, map)) : F.none<ICtx>();
        },
        ictx => {
          var ctx = ((Ctx<Iter<A>, Fn<A, B>, Unit>) ictx);
          return ctx._2(~ctx._1);
        },
        ictx => ((Ctx<Iter<A>, Fn<A, B>, Unit>) ictx)._1.elementsLeft
      ))).iter(Ctx.a(iter, __mapper));
    }

    /* Maps type A to types B Iter (multiple values) and flattens it to an 
     * Iter. */
    public static Iter<B> flatMap<A, B>(this Iter<A> _iter, Fn<A, Iter<B>> __mapper) {
      var cache = Iter<B>.fnCache("#flatMap");

      // We might be empty.
      if (! _iter) return cache.empty;

      var _subIter = __mapper(~_iter);
      // Find first sub iterator which isn't empty.
      while (_iter && ! _subIter) {
        _iter++;
        if (_iter) _subIter = __mapper(~_iter);
      }

      // We might not have any of these, in which case our iter is invalid.
      // ReSharper disable once HeuristicUnreachableCode
      if (! _iter) return cache.empty;

      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<Iter<A>, Iter<B>, Fn<A, Iter<B>>>) ictx);
          var i = ctx._1; var bi = ctx._2; var map = ctx._3;
          if (++bi) {
            // Subiterator has a value.
            return F.some(Ctx.a(i, bi, map));
          }
          else {
            // Find next point where a value exists.
            i++;
            bi = i ? map(~i) : Iter<B>.empty;
            while (i && ! bi) {
              i++;
              if (i) bi = map(~i);
            }

            return bi ? F.some(Ctx.a(i, bi, map)) : F.none<ICtx>();
          }
        },
        ictx => ((Ctx<Iter<A>, Iter<B>, Fn<A, Iter<B>>>) ictx)._2.get,
        ctx => F.none<int>()
      ))).iter(Ctx.a(_iter, _subIter, __mapper));
    }

    /* Skips N elements from this Iter. */
    public static Iter<A> skip<A>(this Iter<A> iter, int count) {
      var i = iter;
      for (var idx = 0; idx < count; idx++) i++;
      return i;
    }

    /* Takes N elements from this Iter. */
    public static Iter<A> take<A>(this Iter<A> __iter, int __count) {
      var cache = Iter<A>.fnCache("#take");
      if (! __iter || __count < 1) return cache.empty;

      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<Iter<A>, int, int>) ictx);
          var iter = ctx._1; var taken = ctx._2; var max = ctx._3;
          return (taken < max && ++iter)
            ? F.some(Ctx.a(iter, taken + 1, max)) : F.none<ICtx>();
        },
        ictx => ((Ctx<Iter<A>, int, int>) ictx)._1.get,
        ictx => {
          var ctx = ((Ctx<Iter<A>, int, int>) ictx);
          var iter = ctx._1; var taken = ctx._2; var max = ctx._3;
          var leftOpt = iter.elementsLeft;
          var takeLeft = max - taken + 1;
          return F.some(
            leftOpt.isEmpty ? takeLeft : Math.Min(leftOpt.get, takeLeft)
          );
        } 
      ))).iter(Ctx.a(__iter, 1, __count) /* iter, taken, max */);
    }

    /* Iterates through Iter and returns first element that satisfies predicate. */
    public static Option<A> find<A>(this Iter<A> iter, Fn<A, bool> predicate) {
      for (var i = iter; i; i++) {
        var current = ~i;
        if (predicate(current)) return F.some(current);
      }
      return F.none<A>();
    }

    /* Iterates through Iter and returns first element that satisfies predicate. */
    public static Option<A> find<A>(this Iter<A> iter, Fn<A, int, bool> predicate) {
      var idx = 0;
      for (var i = iter; i; i++, idx++) {
        var current = ~i;
        if (predicate(current, idx)) return F.some(current);
      }
      return F.none<A>();
    }

    /* Iterates through Iter and returns first Some that finder returns. */
    public static Option<B> find<A, B>(this Iter<A> iter, Fn<A, Option<B>> finder) {
      for (var i = iter; i; i++) {
        var current = ~i;
        var found = finder(current);
        if (found.isDefined) return found;
      }
      return F.none<B>();
    }

    /* Iterates through Iter and returns first Some that finder returns. */
    public static Option<B> find<A, B>(
      this Iter<A> iter, Fn<A, int, Option<B>> finder
    ) {
      var idx = 0;
      for (var i = iter; i; i++, idx++) {
        var current = ~i;
        var found = finder(current, idx);
        if (found.isDefined) return found;
      }
      return F.none<B>();
    }

    /**
     * Iterates the collection. Tries to find a member using predicate. If it
     * doesn't find one, returns last element of enumerable.
     * 
     * Returns None if enumerable is empty.
     **/
    public static Option<A> findOrLast<A>(this Iter<A> iter, Fn<A, bool> predicate) {
      var last = F.none<A>();
      for (var i = iter; i; i++) {
        var current = ~i;
        var currentOpt = F.some(current);
        if (predicate(current)) return currentOpt; 
        else last = currentOpt;
      }
      return last;
    }

    /* Returns first index where predicate matched. 
     * Returns None on empty Iter. */
    public static Option<int> indexWhere<A>(this Iter<A> iter, Fn<A, bool> predicate) {
      var idx = 0;
      for (var i = iter; i; i++, idx++) if (predicate(~i)) return F.some(idx);
      return F.none<int>();
    }

    /* Returns a random element. The probability is selected by elements 
     * weight. */
    public static Option<A> randomElementByWeight<A>(
      this Iter<A> iter, 
      // If we change to Func here, Unity crashes. So fun.
      Fn<A, float> weightSelector
    ) {
      var totalWeightOpt = iter.reduceLeft(weightSelector, (s, i) => s + weightSelector(i));
      if (totalWeightOpt.isEmpty) return F.none<A>();

      var totalWeight = totalWeightOpt.get;
      // The weight we are after...
      var itemWeightIndex = Random.value * totalWeight;
      var currentWeightIndex = 0f;

      for (var i = iter; i; i++) {
        var a = ~i;
        currentWeightIndex += weightSelector(a);
        // If we've hit or passed the weight we are after for this item then it's the one we want....
        if (currentWeightIndex >= itemWeightIndex) return F.some(a);
      }

      throw new IllegalStateException();
    }

    #endregion

    #region Checks

    /* Checks if any element in this Iter satisfies given predicate. 
     * Returns false on empty Iter. */
    public static bool exists<A>(this Iter<A> iter, Fn<A, bool> predicate) {
      for (var i = iter; i; i++) if (predicate(~i)) return true;
      return false;
    }

    /* Checks if given element is in this Iter. 
     * Returns false on empty Iter. */
    public static bool contains<A>(this Iter<A> iter, A element) {
      for (var i = iter; i; i++) 
        if (EqComparer<A>.Default.Equals(~i, element)) return true;
      return false;
    }

    /* Checks if all elements in this Iter satisfies given predicate. 
     * Returns true on empty Iter. */
    public static bool forall<A>(this Iter<A> iter, Fn<A, bool> predicate) {
      for (var i = iter; i; i++) if (! predicate(~i)) return false;
      return true;
    }

    #endregion

    #region Joins

    /* Zips two Iters. Emits values while this or other is exhausted. */
    public static Iter<Tpl<A, B>> zip<A, B>(this Iter<A> __iter, Iter<B> __other) {
      var cache = Iter<Tpl<A, B>>.fnCache("#zip");
      if (! __iter || ! __other) return cache.empty;

      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<Iter<A>, Iter<B>, Unit>) ictx);
          var i1 = ctx._1; var i2 = ctx._2;
          return (++i1).hasValue && (++i2).hasValue
            ? F.some(Ctx.a(i1, i2)) : F.none<ICtx>();
        },
        ictx => {
          var ctx = ((Ctx<Iter<A>, Iter<B>, Unit>) ictx);
          return F.t(~ctx._1, ~ctx._2);
        },
        ictx => {
          var ctx = ((Ctx<Iter<A>, Iter<B>, Unit>) ictx);
          var i1 = ctx._1; var i2 = ctx._2;
          return i1.elementsLeft.zip(i2.elementsLeft).
            map(t => Math.Min(t._1, t._2));
        }
      ))).iter(Ctx.a(__iter, __other));
    }

    /* Returns new Iter that emits (value, index) tuples. */
    public static Iter<Tpl<A, int>> zipWithIndex<A>(this Iter<A> __iter) {
      var cache = Iter<Tpl<A, int>>.fnCache("#zipWithIndex");
      if (! __iter) return cache.empty;
      return (cache.fns ?? (cache.fns = cache.build(
        ictx => {
          var ctx = ((Ctx<Iter<A>, int, Unit>) ictx);
          var iter = ctx._1; var idx = ctx._2;
          return (++iter).hasValue
            ? F.some(Ctx.a(iter, idx + 1)) : F.none<ICtx>();
        },
        ictx => {
          var ctx = ((Ctx<Iter<A>, int, Unit>) ictx);
          return F.t(~ctx._1, ctx._2);
        },
        ictx => ((Ctx<Iter<A>, int, Unit>) ictx)._1.elementsLeft
      ))).iter(Ctx.a(__iter, 0));
    }

    #endregion
    
    #region Context Adding

/**
 * Scala code to generate this part.
 * 
 
import java.io._

val iterCtx = new PrintWriter("IterCtx.cs")

val max = 2
(1 to max).foreach { i =>
  val paramsA = (1 to i).map(s => s"P$s")
  val params = paramsA.mkString(", ")
  val castParams = (
    paramsA ++ Seq.fill(max - i)("Unit")
  ).mkString(", ")
  val args = (1 to i).map(s => s"P$s _p$s").mkString(", ")
  val argVals = (1 to i).map(s => s"_p$s").mkString(", ")
  val ctxArgs = (1 to i).map(s => s"ctx._${s + 1}").mkString(", ")
  val cast = s"((Ctx<Iter<A>, $castParams>) ictx)"
  val emit = s"Tpl<A, $params>"
  
  iterCtx.println(s"""
  // Wrap Iter into a context with $i parameters.
  // Useful for avoiding heap alocations.
  public static Iter<$emit> ctx<A, $params>(this Iter<A> iter, $args) {
    var cache = Iter<$emit>.fnCache("#ctx$i");

    if (! iter) return cache.empty;

    return (cache.fns ?? (cache.fns = cache.build(
      ictx => {
        var ctx = $cast; var i = ctx._1;
        return ++i ? F.some(Ctx.a(i, $ctxArgs)) : F.none<ICtx>();
      },
      ictx => {
        var ctx = $cast;
        return F.t(~ctx._1, $ctxArgs);
      },
      ictx => $cast._1.elementsLeft
    ))).iter(Ctx.a(iter, $argVals));
  }
  """)
}
  
iterCtx.close()
 
 * 
 **/

  // Wrap Iter into a context with 1 parameters.
  // Useful for avoiding heap alocations.
  public static Iter<Tpl<A, P1>> ctx<A, P1>(this Iter<A> iter, P1 _p1) {
    var cache = Iter<Tpl<A, P1>>.fnCache("#ctx1");

    if (! iter) return cache.empty;

    return (cache.fns ?? (cache.fns = cache.build(
      ictx => {
        var ctx = ((Ctx<Iter<A>, P1, Unit>) ictx); var i = ctx._1;
        return ++i ? F.some(Ctx.a(i, ctx._2)) : F.none<ICtx>();
      },
      ictx => {
        var ctx = ((Ctx<Iter<A>, P1, Unit>) ictx);
        return F.t(~ctx._1, ctx._2);
      },
      ictx => ((Ctx<Iter<A>, P1, Unit>) ictx)._1.elementsLeft
    ))).iter(Ctx.a(iter, _p1));
  }
  

  // Wrap Iter into a context with 2 parameters.
  // Useful for avoiding heap alocations.
  public static Iter<Tpl<A, P1, P2>> ctx<A, P1, P2>(this Iter<A> iter, P1 _p1, P2 _p2) {
    var cache = Iter<Tpl<A, P1, P2>>.fnCache("#ctx2");

    if (! iter) return cache.empty;

    return (cache.fns ?? (cache.fns = cache.build(
      ictx => {
        var ctx = ((Ctx<Iter<A>, P1, P2>) ictx); var i = ctx._1;
        return ++i ? F.some(Ctx.a(i, ctx._2, ctx._3)) : F.none<ICtx>();
      },
      ictx => {
        var ctx = ((Ctx<Iter<A>, P1, P2>) ictx);
        return F.t(~ctx._1, ctx._2, ctx._3);
      },
      ictx => ((Ctx<Iter<A>, P1, P2>) ictx)._1.elementsLeft
    ))).iter(Ctx.a(iter, _p1, _p2));
  }
  
    #endregion

// ReSharper restore RedundantTypeArgumentsOfMethod
  }
}
