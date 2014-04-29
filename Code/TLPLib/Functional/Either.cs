using System;

namespace com.tinylabproductions.TLPLib.Functional {
  /** 
  * Hack to not lose covariance of Either.
  **/
  public static class Either {
    public static Either<C, B> flatMapLeft<A, B, C>(
      this Either<A, B> either, Fn<A, Either<C, B>> mapper
    ) { return either.fold(mapper, F.right<C, B>); }

    public static Either<A, C> flatMapRight<A, B, C>(
      this Either<A, B> either, Fn<B, Either<A, C>> mapper
    ) { return either.fold(F.left<A, C>, mapper); }
  }

  public interface Either<out A, out B> {
    bool isLeft { get; }
    bool isRight { get; }
    Option<A> leftValue { get; }
    Option<B> rightValue { get; }

    Either<C, B> mapLeft<C>(Fn<A, C> mapper);
    Either<A, C> mapRight<C>(Fn<B, C> mapper);

    C fold<C>(Fn<A, C> onLeft, Fn<B, C> onRight);
  }

  class Left<A, B> : Either<A, B> {
    private readonly A value;

    public Left(A value) {
      this.value = value;
    }

    public bool isLeft { get { return true; } }
    public bool isRight { get { return false; } }
    public Option<A> leftValue { get { return F.some(value); } }
    public Option<B> rightValue { get { return F.none<B>(); } }

    public Either<C, B> mapLeft<C>(Fn<A, C> mapper) {
      return F.left<C, B>(mapper(value));
    }

    public Either<A, C> mapRight<C>(Fn<B, C> mapper) {
      return F.left<A, C>(value);
    }

    public C fold<C>(Fn<A, C> onLeft, Fn<B, C> onRight) {
      return onLeft(value);
    }
  }

  class Right<A, B> : Either<A, B> {
    private readonly B value;

    public Right(B value) {
      this.value = value;
    }

    public bool isLeft { get { return false; } }
    public bool isRight { get { return true; } }
    public Option<A> leftValue { get { return F.none<A>(); } }
    public Option<B> rightValue { get { return F.some(value); } }

    public Either<C, B> mapLeft<C>(Fn<A, C> mapper) {
      return F.right<C, B>(value);
    }

    public Either<A, C> mapRight<C>(Fn<B, C> mapper) {
      return F.right<A, C>(mapper(value));
    }

    public C fold<C>(Fn<A, C> onLeft, Fn<B, C> onRight) {
      return onRight(value);
    }
  }
}
