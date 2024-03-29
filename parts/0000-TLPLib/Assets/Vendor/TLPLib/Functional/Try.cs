﻿using System;

namespace com.tinylabproductions.TLPLib.Functional {
  public 
#if UNITY_IOS
	class
#else
	struct 
#endif
	Try<A> {

    private readonly A _value;
    private readonly Exception _exception;

#if UNITY_IOS
	public Try() {}
#endif

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

    public B fold<B>(Fn<A, B> onValue, Fn<Exception, B> onException) {
      return isSuccess ? onValue(_value) : onException(_exception);
    }

    public void voidFold(Act<A> onValue, Act<Exception> onException) {
      if (isSuccess) onValue(_value); else onException(_exception);
    }

    public Try<B> map<B>(Fn<A, B> onValue) 
    { return flatMap(a => F.scs(onValue(a))); }

    public Try<B> flatMap<B>(Fn<A, Try<B>> onValue) 
    { return isSuccess ? onValue(_value) : F.err<B>(_exception); }

    public A getOrThrow 
      { get { return isSuccess ? _value : F.throws<A>(_exception); } }

    public override string ToString() {
      return isSuccess ? "Success(" + _value + ")" : "Error(" + _exception + ")";
    }
  }
}
