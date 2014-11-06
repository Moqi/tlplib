using System;

namespace com.tinylabproductions.TLPLib.Tween {
  public class ThresholdChecker {
    public delegate float GetThreshold();
    public delegate bool CheckThreshold(float last, float current);

    public static CheckThreshold minimize = (last, cur) => cur >= last;
    public static CheckThreshold maximize = (last, cur) => cur <= last;

    readonly CheckThreshold checkThreshold;
    readonly GetThreshold getThreshold;
    readonly Act onThresholdReached;

    float threshold;
    bool thresholdMet;

    public ThresholdChecker(
      GetThreshold getThreshold, CheckThreshold checkThreshold,
      Act onThresholdReached
    ) {
      this.getThreshold = getThreshold;
      this.checkThreshold = checkThreshold;
      this.onThresholdReached = onThresholdReached;
      threshold = getThreshold();
    }

    public void check() {
      var curThreshold = getThreshold();
      if (thresholdMet && threshold != curThreshold) thresholdMet = false;
//      Log.trace(string.Format("ThresholdChecker: last={0} current={1}", threshold, curThreshold));
      if (! thresholdMet && checkThreshold(threshold, curThreshold)) {
        //        Log.trace("ThresholdChecker: threshold reached");
        thresholdMet = true;
        onThresholdReached();
      }
      threshold = curThreshold;
    }
  }
}
