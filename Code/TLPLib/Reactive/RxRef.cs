using System;
using System.Collections.Generic;

namespace com.tinylabproductions.TLPLib.Reactive {
  /**
   * RxVal is an observable which has a current value.
   **/
  public interface IRxVal<out A> : IObservable<A> {
    A value { get; }
  }

  /**
   * RxRef is a reactive reference, which stores a value and also acts as a IObserver.
   **/
  public interface IRxRef<A> : IRxVal<A>, IObserver<A> {
    new A value { get; set; }
  }

  public static class RxRef {
    public static IRxRef<A> a<A>(A value) {
      return new RxRef<A>(value);
    }
  }

  public class RxRef<A> : Observable<A>, IRxRef<A> {
    private static ObserverBuilder<Elem, RxRef<Elem>> builder<Elem>(Elem value) {
      return builder => {
        var rxRef = new RxRef<Elem>(value);
        builder(rxRef);
        return rxRef;
      };
    }

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

    public void push(A pushedValue) { value = pushedValue; }
  }
}
