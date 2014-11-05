using System;

namespace com.tinylabproductions.TLPLib.Tween {
  public struct ThresholdChecker {
    public delegate float GetThreshold();
    public delegate bool CheckThreshold(float last, float current);

    public static CheckThreshold minimize = (last, cur) => last > cur;
    public static CheckThreshold maximize = (last, cur) => last < cur;

    private readonly CheckThreshold checkThreshold;
    private readonly GetThreshold getThreshold;
    private readonly Act onThresholdReached;

    private float threshold;

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
      if (checkThreshold(threshold, curThreshold)) onThresholdReached();
      threshold = curThreshold;
    }
  }
}
