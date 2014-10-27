using System;
using com.tinylabproductions.TLPLib.Concurrent;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Tween {
  /* Future which completes when the tween completes. */
  public interface ITweenFuture : Future<Unit> {
    void destroy();
  }

  public delegate AbstractGoTween TweenFutureCreator(GoTweenConfig config);

  public static class TweenFuture {
    public static ITweenFuture a(TweenFutureCreator creator) {
      return new TweenFutureImpl(creator);
    }

    public static readonly ITweenFuture noTween = new TweenFutureCompleted();
  }

  class TweenFutureImpl : FutureImpl<Unit>, ITweenFuture {
    private readonly AbstractGoTween tween;

    internal TweenFutureImpl(TweenFutureCreator creator) {
      tween = creator(
        new GoTweenConfig().startPaused().onComplete(t => completeSuccess(F.unit))
      );
      tween.play();
    }

    public void destroy() { tween.destroy(); }
  }

  class TweenFutureCompleted : ITweenFuture {
    public Option<Unit> pureValue { get { return F.some(F.unit); } }
    public Option<Try<Unit>> value { get { return pureValue.map(F.scs); } }

    public CancellationToken onComplete(Act<Try<Unit>> action) {
      action(F.scs(F.unit));
      return Future.FinishedCancellationToken.instance;
    }

    public CancellationToken onSuccess(Act<Unit> action) {
      action(F.unit);
      return Future.FinishedCancellationToken.instance;
    }

    public CancellationToken onFailure(Act<Exception> action) {
      return Future.FinishedCancellationToken.instance;
    }

    public void destroy() {}
  }
}
