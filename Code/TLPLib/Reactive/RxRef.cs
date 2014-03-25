using System;
using System.Collections.Generic;

namespace com.tinylabproductions.TLPLib.Reactive {
  /**
   * RxVal is an observable which has a current value.
   **/
  public interface IRxVal<A> : IObservable<A> {
    A value { get; }
    new IRxVal<B> map<B>(Fn<A, B> mapper);
  }

  /**
   * RxRef is a reactive reference, which stores a value and also acts as a IObserver.
   **/
  public interface IRxRef<A> : IRxVal<A>, IObserver<A> {
    new A value { get; set; }
  }

  public static class RxVal {
    public static ObserverBuilder<Elem, IRxVal<Elem>> builder<Elem>(Elem value) {
      // Unity Mono doesn't quite understand that it is the same type :|
      // return RxRef.builder(value);
      return builder => {
        var rxRef = new RxRef<Elem>(value);
        builder(rxRef);
        return (IRxVal<Elem>) rxRef;
      };
    }
  }

  public static class RxRef {
    public static ObserverBuilder<Elem, IRxRef<Elem>> builder<Elem>(Elem value) {
      return builder => {
        var rxRef = new RxRef<Elem>(value);
        builder(rxRef);
        return rxRef;
      };
    }

    public static IRxRef<A> a<A>(A value) {
      return new RxRef<A>(value);
    }
  }

  public class RxRef<A> : Observable<A>, IRxRef<A> {
    private A _value;
    public A value { 
      get { return _value; }
      set {
        if (EqualityComparer<A>.Default.Equals(_value, value)) return;
        _value = value;
        submit(value);
      }
    }

    public RxRef(A initialValue) {
      _value = initialValue;
    }

    public override ISubscription subscribe(Act<A> onChange) {
      var subscription = base.subscribe(onChange);
      onChange(value); // Emit current value on subscription.
      return subscription;
    }

    public new IRxVal<B> map<B>(Fn<A, B> mapper) {
      return mapImpl(mapper, RxVal.builder(mapper(value)));
    }

    public void push(A pushedValue) { value = pushedValue; }
  }
}
