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

    public static Either<C, B> mapLeft<A, B, C>(
      this Either<A, B> either, Fn<A, C> mapper
    ) {
      return either.fold(v => F.left<C, B>(mapper(v)), F.right<C, B>);
    }

    public static Either<A, C> mapRight<A, B, C>(
      this Either<A, B> either, Fn<B, C> mapper
    ) {
      return either.fold(F.left<A, C>, v => F.right<A, C>(mapper(v)));
    }

    public static C fold<A, B, C>(
      this Either<A, B> either, Fn<A, C> onLeft, Fn<B, C> onRight
    ) {
      return either.rightValue.
        fold(() => onLeft(either.leftValue.get), onRight);
    }
  }

  public interface Either<out A, out B> {
    bool isLeft { get; }
    bool isRight { get; }
    Option<A> leftValue { get; }
    Option<B> rightValue { get; }
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

    public override string ToString() {
      return string.Format("Left({0})", value);
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

    public override string ToString() {
      return string.Format("Right({0})", value);
    }
  }
}
