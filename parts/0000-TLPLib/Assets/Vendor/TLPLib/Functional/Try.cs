using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public struct Try<A> {
    private readonly A _value;
    private readonly Exception _exception;

    public Try(A value) { 
      _value = value;
      _exception = null;
    }

    public Try(Exception ex) {
      _value = default(A);
      _exception = ex;
    }

    public bool isSuccess { get { return _exception == null; } }
    public bool isError { get { return _exception != null; } }

    public Option<A> value { get {
      return isSuccess ? F.some(_value) : F.none<A>();
    } }

    public Option<Exception> exception { get {
      return isSuccess ? F.none<Exception>() : F.some(_exception);
    } }

    public void voidFold(Act<A> onValue, Act<Exception> onException) {
      if (isSuccess) onValue(_value); else onException(_exception);
    }

    public A getOrThrow 
      { get { return isSuccess ? _value : F.throws<A>(_exception); } }

    public override string ToString() {
      return isSuccess ? "Success(" + _value + ")" : "Error(" + _exception + ")";
    }
  }

  public static class TryExts {
    public static B fold<A, B>(
      this Try<A> t, Fn<A, B> onValue, Fn<Exception, B> onException
    ) {
      return t.isSuccess ? onValue(t.value.get) : onException(t.exception.get);
    }

    public static Try<B> map<A, B>(
      this Try<A> t, Fn<A, B> onValue
    ) { return t.flatMap(a => F.scs(onValue(a))); }

    public static Try<B> flatMap<A, B>(this Try<A> t, Fn<A, Try<B>> onValue) 
    { return t.isSuccess ? onValue(t.value.get) : F.err<B>(t.exception.get); }
  }
}
