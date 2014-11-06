using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Logger;

namespace com.tinylabproductions.TLPLib.Tween {
  /* Threshold checker that iterates through given enumerable when threshold 
   * is met. */
  public static class EnumerableThresholdChecker {
    public delegate void ThresholdReached<in A>(int index, A a);

    public static EnumerableThresholdChecker<A> a<A>(
      IEnumerable<A> enumerable, Fn<A, float> toThreshold,
      ThresholdChecker.CheckThreshold checkerFn,
      ThresholdReached<A> onThresholdReached
    ) {
      return new EnumerableThresholdChecker<A>(enumerable, toThreshold, checkerFn, onThresholdReached);
    }
  }

  public class EnumerableThresholdChecker<A> {
    private readonly IEnumerator<A> enumerator;
    private readonly Fn<A, float> toThreshold;
    private readonly ThresholdChecker.CheckThreshold checkerFn;
    private readonly EnumerableThresholdChecker.ThresholdReached<A> onThresholdReached;

    private Option<ThresholdChecker> checker;
    private int index = -1;

    public EnumerableThresholdChecker(
      IEnumerable<A> enumerable, Fn<A, float> toThreshold,
      ThresholdChecker.CheckThreshold checkerFn,
      EnumerableThresholdChecker.ThresholdReached<A> onThresholdReached
    ) {
      enumerator = enumerable.GetEnumerator();
      this.toThreshold = toThreshold;
      this.checkerFn = checkerFn;
      this.onThresholdReached = onThresholdReached;
      checker = F.none<ThresholdChecker>();
      next();
    }

    private void next() {
      if (enumerator.MoveNext()) {
        index++;
        var a = enumerator.Current;
        Log.debug(string.Format(
          "EnumerableThresholdChecker: threshold reached, switching to next element [idx={0}]: {1}",
          index, a
        ));
        checker = F.some(new ThresholdChecker(
          () => toThreshold(a), checkerFn, () => {
            onThresholdReached(index, a);
            next();
          }
        ));
      }
      else {
        checker = F.none<ThresholdChecker>();
      }
    }

    public void check() { checker.each(_ => _.check()); }
  }
}
