using System;
using Smooth.Collections;

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
    /** Returns a new ref that is bound to this ref and vice versa. **/
    IRxRef<B> comap<B>(Fn<A, B> mapper, Fn<B, A> comapper);
  }

  /* RxRef for mutable values. Cannot be changed, because the object itself is mutable. */
  public interface IRxMutRef<A> : IRxVal<A> {
    /* Execute change function and notify subscribers about the change. */
    A change(Act<A> change);
    /* Notify subscribers that the value inside has mutated. */
    void changed();
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

  public abstract class RxRefBase<A> : Observable<A> {
    protected A _value;
    public A value { get { return _value; } }

    protected RxRefBase(A initialValue) { _value = initialValue; }

    public new IRxVal<B> map<B>(Fn<A, B> mapper) {
      return mapImpl(mapper, RxVal.builder(mapper(value)));
    }

    public override ISubscription subscribe(Act<A> onChange) {
      var subscription = base.subscribe(onChange);
      onChange(value); // Emit current value on subscription.
      return subscription;
    }
  }

  public class RxRef<A> : RxRefBase<A>, IRxRef<A> {
    public new A value { 
      get { return _value; }
      set {
        if (EqComparer<A>.Default.Equals(_value, value)) return;
        _value = value;
        submit(value);
      }
    }

    public RxRef(A initialValue) : base(initialValue) {}

    public IRxRef<B> comap<B>(Fn<A, B> mapper, Fn<B, A> comapper) {
      var bRef = mapImpl(mapper, RxRef.builder(mapper(value)));
      bRef.subscribe(b => value = comapper(b));
      return bRef;
    }

    public void push(A pushedValue) { value = pushedValue; }
  }

  public class RxMutRef {
    public static IRxMutRef<A> a<A>(A value) { return new RxMutRef<A>(value); }
  }

  public class RxMutRef<A> : RxRefBase<A>, IRxMutRef<A> {
    public RxMutRef(A initialValue) : base(initialValue) {}

    public A change(Act<A> change) {
      change(_value);
      changed();
      return _value;
    }

    public void changed() { submit(_value); }
  }
}
