using System;
using System.Collections.Generic;
using System.Text;
using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Logger;
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
  public struct Iter<A, Ctx> {
    #region Internal state

    /* Takes a context and provides next context, which is None if Iter is 
     * done. */
    private readonly Fn<Ctx, Option<Ctx>> skipper;
    /* Extracts a value from context. */
    private readonly Fn<Ctx, A> getter;
    /* How many elements are left in this Iter? */
    private readonly Fn<Ctx, Option<int>> sizeHint;

    /* None if Iter is finished, Some if it's valid. */
    private Option<Ctx> state;

    #endregion

    #region Base interface

    public static readonly Iter<A, Ctx> empty = new Iter<A, Ctx>(
      F.none<Ctx>(), _ => F.none<Ctx>(), _ => default(A), _ => F.none<int>()
    );

    public Iter(
      Option<Ctx> state, Fn<Ctx, Option<Ctx>> skipper, Fn<Ctx, A> getter, 
      Fn<Ctx, Option<int>> sizeHint
    ) {
      this.state = state;
      this.skipper = skipper;
      this.getter = getter;
      this.sizeHint = sizeHint;
    }

    public Iter(
      Ctx ctx, Fn<Ctx, Option<Ctx>> skipper, Fn<Ctx, A> getter,
      Fn<Ctx, Option<int>> sizeHint
    ) : this(F.some(ctx), skipper, getter, sizeHint) {}

    public override string ToString() {
      return string.Format("Iter({0}|sh:{1})", state, elementsLeft);
    }

    /* Mutates Iter and moves it to next place in the sequence. */
    public void progress() { state = state.flatMap(skipper); }

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
    public static implicit operator bool(Iter<A, Ctx> iter) { return iter.hasValue; }

    /* Alias to .get */
    public static A operator ~(Iter<A, Ctx> iter) { return iter.get; }

    /* Alias to .skip() */
    public static Iter<A, Ctx> operator ++(Iter<A, Ctx> iter) {
      iter.progress();
      return iter;
    }

    #endregion

    #region Conversions

    /* Iterates underlying sequence and returns a list from that. */
    public List<A> toList() {
      var list = elementsLeft.fold(() => new List<A>(), F.emptyList<A>);
      for (var i = this; i; i++) list.Add(~i);
      return list;
    }

    /* Same as toList() but returns an interface type. */
    public IList<A> toIList() { return toList(); }

    /* Iterates underlying sequence and returns a linked list from that. */
    public LinkedList<A> toLinkedList() {
      var ll = new LinkedList<A>();
      for (var i = this; i; i++) ll.AddLast(~i);
      return ll;
    }

    /* Iterates underlying sequence and returns an array from that.
     * 
     * Beware that this will cause in a temporary list for sources that do not
     * provide a size hint. */
    public A[] toArray() {
      var leftOpt = elementsLeft;
      if (leftOpt.isDefined) {
        var arr = new A[leftOpt.get];
        var idx = 0;
        for (var i = this; i; i++, idx++) arr[idx] = ~i;
        return arr;
      }
      else {
        var list = new List<A>();
        for (var i = this; i; i++) list.Add(~i);
        return list.ToArray();
      }
    }

    #endregion

    #region Context Adding

    public Iter<Tpl<A, P1>, Tpl<Iter<A, Ctx>, P1>> ctx<P1>(P1 _p1) {
      if (! this) return Iter<Tpl<A, P1>, Tpl<Iter<A, Ctx>, P1>>.empty;

      return new Iter<Tpl<A, P1>, Tpl<Iter<A, Ctx>, P1>>(
        F.t(this, _p1),
        ctx => { var i = ctx._1;
          return ++i ? F.some(F.t(i, ctx._2)) : F.none<Tpl<Iter<A, Ctx>, P1>>();
        },
        ctx => F.t(~ctx._1, ctx._2),
        ctx => ctx._1.elementsLeft
      );
    }

    public Iter<Tpl<A, P1, P2>, Tpl<Iter<A, Ctx>, P1, P2>> ctx<P1, P2>(P1 _p1, P2 _p2) {
      if (! this) return Iter<Tpl<A, P1, P2>, Tpl<Iter<A, Ctx>, P1, P2>>.empty;

      return new Iter<Tpl<A, P1, P2>, Tpl<Iter<A, Ctx>, P1, P2>>(
        F.t(this, _p1, _p2),
        ctx => { var i = ctx._1;
          return ++i ? F.some(F.t(i, ctx._2, ctx._3)) : F.none<Tpl<Iter<A, Ctx>, P1, P2>>();
        },
        ctx => F.t(~ctx._1, ctx._2, ctx._3),
        ctx => ctx._1.elementsLeft
      );
    }

    #endregion

    #region Folds

    /* Folds this Iter to a value using given initial state and folder. */
    public B foldLeft<B>(B initial, Fn<B, A, B> folder) {
      var current = initial;
      for (var i = this; i; i++) current = folder(current, ~i);
      return current;
    }

    /* Reduces this Iter to a value using first element as initial 
     * state and given reducer.
     * 
     * Returns None if Iter is empty. */
    public Option<A> reduceLeft(Fn<A, A, A> reducer) {
      return reduceLeft(_ => _, reducer);
    }

    /* Reduces this Iter to a value using first element passed through 
     * initial state extractor as initial state and given reducer.
     * 
     * Returns None if Iter is empty. */
    public Option<B> reduceLeft<B>(
      Fn<A, B> initialStateExtractor, Fn<B, A, B> reducer
    ) {
      if (this) {
        var i = this;
        var initial = ~(i++);
        return F.some(i.foldLeft(initialStateExtractor(initial), reducer));
      }
      else return F.none<B>();
    }

    /* Reduces this Iter to a value using a predicate that tells us whether
     * we should keep left argument.
     * 
     * Returns None if Iter is empty. */
    public Option<A> keepLeft(Fn<A, A, bool> keepLeft) {
      return reduceLeft((a, b) => keepLeft(a, b) ? a : b);
    }

    /**
     * Returns tuple of linked lists where first one contains all the items
     * that matched the predicate and second - those who didn't.
     **/
    public Tpl<LinkedList<A>, LinkedList<A>> partition(Fn<A, bool> predicate) {
      var trues = new LinkedList<A>();
      var falses = new LinkedList<A>();
      for (var i = this; i; i++) {
        var a = ~i;
        if (predicate(a)) trues.AddLast(a);
        else falses.AddLast(a);
      }
      return F.t(trues, falses);
    }

    /* Iterates through sequence, extracts a float and returns average. 
     * Returns None if Iter is empty. */
    public Option<float> avg(Fn<A, float> extractor) {
      if (! this) return F.none<float>();

      var total = 0f;
      var count = 0;

      for (var i = this; i; i++) {
        total += extractor(~i);
        count += 1;
      }

      return F.some(total / count);
    }

    /* Constructs a string from this Iter. */
    public string mkString(
      string separator, string start = null, string end = null
    ) {
      var b = new StringBuilder();
      if (start != null) b.Append(start);
      var idx = 0;
      for (var i = this; i; i++, idx++) {
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
    public void each(Act<A> f) { for (var i = this; i; i++) f(~i); }

    /* Lambda style for each with index. */
    public void eachWithIndex(Act<A, int> f) {
      var idx = 0;
      for (var i = this; i; i++, idx++) f(~i, idx);
    }

    /* Filters Iter with given predicate. */
    public Iter<A, Tpl<Iter<A, Ctx>, Fn<A, bool>>>
    filter(Fn<A, bool> predicate) {
      var iter = this;
      // Find first item which satisfies given predicate.
      for (; iter && ! predicate(~iter); iter++) {}

      // If we're done with the iterator, return empty Iter.
      if (! iter) return Iter<A, Tpl<Iter<A, Ctx>, Fn<A, bool>>>.empty;

      // Otherwise return new iterator that filters elements.
      return new Iter<A, Tpl<Iter<A, Ctx>, Fn<A, bool>>>(
        F.t(iter, predicate),
        ctx => { var i = ctx._1; var p = ctx._2;
          // Skip to first next one where predicate is satisfied.
          while ((++i).hasValue && ! p(~i)) {}

          return i 
            ? F.some(F.t(i, p)) 
            : F.none<Tpl<Iter<A, Ctx>, Fn<A, bool>>>();
        },
        ctx => ~ctx._1,
        ctx => F.none<int>()
      );
    }

    /* Maps type A to type B. */
    public Iter<B, Tpl<Iter<A, Ctx>, Fn<A, B>>> map<B>(Fn<A, B> mapper) {
      if (state.isEmpty) return Iter<B, Tpl<Iter<A, Ctx>, Fn<A, B>>>.empty;

      return new Iter<B, Tpl<Iter<A, Ctx>, Fn<A, B>>>(
        F.t(this, mapper),
        ctx => { var i = ctx._1; var map = ctx._2;
          return (++i).hasValue
            ? F.some(F.t(i, map))
            : F.none<Tpl<Iter<A, Ctx>, Fn<A, B>>>();
        },
        ctx => ctx._2(~ctx._1),
        ctx => ctx._1.elementsLeft
      );
    }

    /* Maps type A to types B Iter (multiple values) and flattens it to an 
     * Iter. */
    public Iter<
      B, Tpl<Iter<A, Ctx>, Iter<B, BCtx>, Fn<A, Iter<B, BCtx>>>
    > flatMap<B, BCtx>(Fn<A, Iter<B, BCtx>> mapper) {
      // We might be empty.
      if (! this) return Iter<
        B, Tpl<Iter<A, Ctx>, Iter<B, BCtx>, Fn<A, Iter<B, BCtx>>>
      >.empty;

      var iter = this;
      var subIter = mapper(~iter);
      // Find first sub iterator which isn't empty.
      while (iter && ! subIter) {
        iter++;
        if (iter) subIter = mapper(~iter);
      }

      // We might not have any of these, in which case our iter is invalid.
      if (! iter) return Iter<
        B, Tpl<Iter<A, Ctx>, Iter<B, BCtx>, Fn<A, Iter<B, BCtx>>>
      >.empty;

      return new Iter<
        B, Tpl<Iter<A, Ctx>, Iter<B, BCtx>, Fn<A, Iter<B, BCtx>>>
      >(
        F.t(iter, subIter, mapper),
        ctx => { var i = ctx._1; var bi = ctx._2; var map = ctx._3;
          if (++bi) {
            // Subiterator has a value.
            var nCtx = F.some(F.t(i, bi, map));
            return nCtx;
          }
          else {
            // Find next point where a value exists.
            i++;
            bi = i ? map(~i) : Iter<B, BCtx>.empty;
            while (i && ! bi) {
              i++;
              if (i) bi = map(~i);
            }

            return bi ? F.some(F.t(i, bi, map)) : F.none<
              Tpl<Iter<A, Ctx>, Iter<B, BCtx>, Fn<A, Iter<B, BCtx>>>
            >();
          }
        },
        ctx => ~ctx._2,
        ctx => F.none<int>()
      );
    }

    /* Skips N elements from this Iter. */
    public Iter<A, Ctx> skip(int count) {
      var i = this;
      for (var idx = 0; idx < count; idx++) i++;
      return i;
    }

    /* Takes N elements from this Iter. */
    public Iter<A, Tpl<Iter<A, Ctx>, int, int>> take(int count) {
      if (state.isEmpty || count < 1) 
        return Iter<A, Tpl<Iter<A, Ctx>, int, int>>.empty;

      return new Iter<A, Tpl<Iter<A, Ctx>, int, int>>(
        F.t(this, 1, count), // iter, taken, max
        ctx => ctx.ua((iter, taken, max) => 
          (taken < count && (++iter).hasValue)
            ? F.some(F.t(iter, taken + 1, max))
            : F.none<Tpl<Iter<A, Ctx>, int, int>>()
        ),
        ctx => ~ctx._1,
        ctx => ctx.ua((iter, taken, max) => {
          var leftOpt = iter.elementsLeft;
          var takeLeft = max - taken + 1;
          return F.some(
            leftOpt.isEmpty ? takeLeft : Math.Min(leftOpt.get, takeLeft)
          );
        }) 
      );
    }

    /* Iterates through Iter and returns first element that satisfies predicate. */
    public Option<A> find(Fn<A, bool> predicate) {
      for (var i = this; i; i++) {
        var current = ~i;
        if (predicate(current)) return F.some(current);
      }
      return F.none<A>();
    }

    /* Iterates through Iter and returns first element that satisfies predicate. */
    public Option<A> find(Fn<A, int, bool> predicate) {
      var idx = 0;
      for (var i = this; i; i++, idx++) {
        var current = ~i;
        if (predicate(current, idx)) return F.some(current);
      }
      return F.none<A>();
    }

    /* Iterates through Iter and returns first Some that finder returns. */
    public Option<B> find<B>(Fn<A, Option<B>> finder) {
      for (var i = this; i; i++) {
        var current = ~i;
        var found = finder(current);
        if (found.isDefined) return found;
      }
      return F.none<B>();
    }

    /* Iterates through Iter and returns first Some that finder returns. */
    public Option<B> find<B>(Fn<A, int, Option<B>> finder) {
      var idx = 0;
      for (var i = this; i; i++, idx++) {
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
    public Option<A> findOrLast(Fn<A, bool> predicate) {
      var last = F.none<A>();
      for (var i = this; i; i++) {
        var current = ~i;
        var currentOpt = F.some(current);
        if (predicate(current)) return currentOpt; 
        else last = currentOpt;
      }
      return last;
    }

    /* Returns first index where predicate matched. 
     * Returns None on empty Iter. */
    public Option<int> indexWhere(Fn<A, bool> predicate) {
      var idx = 0;
      for (var i = this; i; i++, idx++) if (predicate(~i)) return F.some(idx);
      return F.none<int>();
    }

    /* Returns a random element. The probability is selected by elements 
     * weight. */
    public Option<A> randomElementByWeight(
      // If we change to Func here, Unity crashes. So fun.
      Fn<A, float> weightSelector
    ) {
      var totalWeightOpt = reduceLeft(weightSelector, (s, i) => s + weightSelector(i));
      if (totalWeightOpt.isEmpty) return F.none<A>();

      var totalWeight = totalWeightOpt.get;
      // The weight we are after...
      var itemWeightIndex = Random.value * totalWeight;
      var currentWeightIndex = 0f;

      for (var i = this; i; i++) {
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
    public bool exists(Fn<A, bool> predicate) {
      for (var i = this; i; i++) if (predicate(~i)) return true;
      return false;
    }

    /* Checks if given element is in this Iter. 
     * Returns false on empty Iter. */
    public bool contains(A element) {
      for (var i = this; i; i++) 
        if (EqComparer<A>.Default.Equals(~i, element)) return true;
      return false;
    }

    /* Checks if all elements in this Iter satisfies given predicate. 
     * Returns true on empty Iter. */
    public bool forall(Fn<A, bool> predicate) {
      for (var i = this; i; i++) if (! predicate(~i)) return false;
      return true;
    }

    #endregion

    #region Joins

    /* Zips two Iters. Emits values while this or other is exhausted. */
    public Iter<Tpl<A, B>, Tpl<Iter<A, Ctx>, Iter<B, BCtx>>> 
    zip<B, BCtx>(Iter<B, BCtx> other) {
      if (! this || ! other) 
        return Iter<Tpl<A, B>, Tpl<Iter<A, Ctx>, Iter<B, BCtx>>>.empty;

      return new Iter<Tpl<A, B>, Tpl<Iter<A, Ctx>, Iter<B, BCtx>>>(
        F.t(this, other),
        ctx => { var i1 = ctx._1; var i2 = ctx._2;
          return (++i1).hasValue && (++i2).hasValue
            ? F.some(F.t(i1, i2))
            : F.none<Tpl<Iter<A, Ctx>, Iter<B, BCtx>>>();
        },
        ctx => F.t(~ctx._1, ~ctx._2),
        ctx => 
          ctx._1.elementsLeft.zip(ctx._2.elementsLeft).
          map(t => Math.Min(t._1, t._2))
      );
    }

    /* Returns new Iter that emits (value, index) tuples. */
    public Iter<Tpl<A, int>, Tpl<Iter<A, Ctx>, int>> zipWithIndex() {
      if (state.isEmpty) return Iter<Tpl<A, int>, Tpl<Iter<A, Ctx>, int>>.empty;
      return new Iter<Tpl<A, int>, Tpl<Iter<A, Ctx>, int>>(
        F.t(this, 0),
        ctx => { var iter = ctx._1; var idx = ctx._2;
          return (++iter).hasValue 
            ? F.some(F.t(iter, idx + 1)) 
            : F.none<Tpl<Iter<A, Ctx>, int>>();
        },
        ctx => F.t(~ctx._1, ctx._2),
        ctx => ctx._1.elementsLeft
      );
    }

    #endregion
  }
}
