using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public interface Try<out A> {
    Option<A> value { get; }
    Option<Exception> exception { get; }
    void voidFold(Act<A> onValue, Act<Exception> onException);
    A getOrThrow { get; }
  }

  public class Success<A> : Try<A> {
    private readonly A _value;

    public Success(A value) {
      _value = value;
    }

    public Option<A> value { get { return F.some(_value); } }
    public Option<Exception> exception { get { return F.none<Exception>(); } }

    public void voidFold(Act<A> onValue, Act<Exception> onException) {
      onValue(_value);
    }

    public A getOrThrow { get { return _value; } }

    public override string ToString() {
      return string.Format("Success({0})", _value);
    }
  }

  public class Error<A> : Try<A> {
    private readonly Exception _exception;

    public Error(Exception exception) {
      _exception = exception;
    }

    public Option<A> value { get { return F.none<A>(); } }
    public Option<Exception> exception { get { return F.some(_exception); } }

    public void voidFold(Act<A> onValue, Act<Exception> onException) {
      onException(_exception);
    }

    public A getOrThrow { get { throw _exception; } }

    public override string ToString() {
      return string.Format("Error({0})", _exception);
    }
  }
}
