using System;
using System.Collections.Generic;

namespace com.tinylabproductions.TLPLib.Reactive {
  public interface ISubscription {
    bool isSubscribed { get; }
    bool unsubscribe();
    ISubscription andThen(Act action);
  }

  public class Subscription : ISubscription {
    private readonly Action onUnsubscribe;
    private bool _isSubscribed = true;

    public Subscription(Action onUnsubscribe) {
      this.onUnsubscribe = onUnsubscribe;
    }

    public bool isSubscribed { get { return _isSubscribed; } }

    public bool unsubscribe() {
      if (!isSubscribed) return false;
      _isSubscribed = false;
      onUnsubscribe();
      return true;
    }

    public ISubscription andThen(Act action) {
      return new Subscription(() => {
        unsubscribe();
        action();
      });
    }
  }

  public class SubscriptionTracker : IDisposable {
    private readonly List<ISubscription> subscriptions =
      new List<ISubscription>();

    public ISubscription track(ISubscription subscription) {
      subscriptions.Add(subscription);
      return subscription;
    }

    public void Dispose() {
      foreach (var subscription in subscriptions)
        subscription.unsubscribe();
      subscriptions.Clear();
    }
  }
}
