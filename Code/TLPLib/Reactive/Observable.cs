using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Collection;
using com.tinylabproductions.TLPLib.Concurrent;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Reactive {
  public interface IObservable<A> {
    ISubscription subscribe(Act<A> onChange);
    /** Emits first value to the future and unsubscribes. **/
    Future<A> toFuture();
    /** Maps events coming from this observable. **/
    IObservable<B> map<B>(Fn<A, B> mapper);
    /** 
     * Maps events coming from this observable and emits all events contained 
     * in returned enumerable.
     **/
    IObservable<B> flatMap<B>(Fn<A, IEnumerable<B>> mapper);
    /** Only emits events that pass the predicate. **/
    IObservable<A> filter(Fn<A, bool> predicate);
    /**
     * Buffers values into a linked list of specified size. Oldest values 
     * are at the front of the buffer.
     **/
    IObservable<ILinkedList<A>> buffer(int size);
    /**
     * Joins events of two observables returning an observable which emits
     * events when either observable emits them.
     **/
    IObservable<A> join<B>(IObservable<B> other) where B : A;
    /** 
     * Only emits an event if other event was not emmited in specified 
     * time range.
     **/
    IObservable<A> onceEvery(float seconds);
    /**
     * Waits until `count` events are emmited within a single `timeframe` 
     * seconds window and emits a read only linked list of 
     * (element, emmision time) tuples with emmission time taken from 
     * `Time.time`.
     **/
    IObservable<ILinkedList<Tpl<A, float>>> withinTimeframe(int count, float timeframe);
    IObservable<Tpl<A, B>> zip<B>(IObservable<B> other);
    // Returns pairs of (old, new) values when they are changing.
    // If there was no events before, old may be None.
    IObservable<Tpl<Option<A>, A>> changesOpt();
    // Like changesOpt() but does not emit if old was None.
    IObservable<Tpl<A, A>> changes();
    // Like changes() but only emits new values.
    IObservable<A> changedValues();
  }

  public interface IObserver<in A> {
    void push(A value);
  }

  public class Observer<A> : IObserver<A> {
    private readonly Act<A> onValuePush;

    public Observer(Act<A> onValuePush) {
      this.onValuePush = onValuePush;
    }

    public void push(A value) {
      onValuePush(value);
    }
  }

  public static class Observable {
    public static Tpl<A, IObservable<Evt>> a<A, Evt>
    (Fn<IObserver<Evt>, A> creator) {
      IObserver<Evt> observer = null;
      var observable = new Observable<Evt>(obs => observer = obs);
      var obj = creator(observer);
      return F.t(obj, (IObservable<Evt>) observable);
    }

    public static IObservable<A> fromEvent<A>(Act<Act<A>> registerCallback) {
      return new Observable<A>(obs => registerCallback(obs.push));
    }

    private static IObservable<Unit> everyFrameInstance;

    public static IObservable<Unit> everyFrame { get {
      return everyFrameInstance ?? (
        everyFrameInstance = new Observable<Unit>(observer =>
          Concurrent.ASync.StartCoroutine(everyFrameCR(observer))
        )
      );
    } }

    public static IObservable<DateTime> interval(
      MonoBehaviour behaviour, float intervalS
      ) {
      return interval(intervalS, F.none<float>());
    }

    public static IObservable<DateTime> interval(
      MonoBehaviour behaviour, float intervalS, float delayS
    ) {
      return interval(intervalS, F.some(delayS));
    }

    public static IObservable<DateTime> interval(
      float intervalS, Option<float> delayS
    ) {
      return new Observable<DateTime>(observer =>
        Concurrent.ASync.StartCoroutine(
          interval(observer, intervalS, delayS)
        )
      );
    }

    public static IObservable<Tpl<P1, P2, P3, P4>> tuple<P1, P2, P3, P4>(
      IObservable<P1> o1, IObservable<P2> o2, IObservable<P3> o3, IObservable<P4> o4
    ) {
      return o1.zip<P2>(o2).zip<P3>(o3).zip<P4>(o4).
        map<Tpl<P1, P2, P3, P4>>(t => F.t(t._1._1._1, t._1._1._2, t._1._2, t._2));
    }

    private static IEnumerator everyFrameCR(IObserver<Unit> observer) {
      while (true) {
        observer.push(Unit.instance);
        yield return null;
      }
    }

    private static IEnumerator interval(
      IObserver<DateTime> observer, float intervalS, Option<float> delayS
    ) {
      if (delayS.isDefined) yield return new WaitForSeconds(delayS.get);
      var wait = new WaitForSeconds(intervalS);
      while (true) {
        observer.push(DateTime.Now);
        yield return wait;
      }
    }
  }

  public delegate ObservableImplementation ObserverBuilder<
    in Elem, out ObservableImplementation
  >(Act<IObserver<Elem>> submitValue);

  public class Observable<A> : IObservable<A> {
    private static ObserverBuilder<Elem, IObservable<Elem>> builder<Elem>() {
      return builder => new Observable<Elem>(builder);
    }

    private readonly IList<Tpl<Subscription, Act<A>>> subscriptions =
      new List<Tpl<Subscription, Act<A>>>();

    protected Observable() {}

    public Observable(Act<IObserver<A>> onSubmit) {
      onSubmit(new Observer<A>(submit));
    }

    protected virtual void submit(A value) {
      // Make a copy of subscriptions to prevent concurrent modification of it.
      var localSubscription = subscriptions.ToArray();
      foreach (var t in localSubscription) t._2(value);
    }

    public virtual ISubscription subscribe(Act<A> onChange) {
      Subscription subscription = null;
      // ReSharper disable once AccessToModifiedClosure
      subscription = new Subscription(() => unsubscribe(subscription));
      subscriptions.Add(F.t(subscription, onChange));
      return subscription;
    }

    public Future<A> toFuture() {
      var f = new FutureImpl<A>();
      var subscription = subscribe(f.complete);
      f.onComplete(_ => subscription.unsubscribe());
      return f;
    }

    public IObservable<B> map<B>(Fn<A, B> mapper) {
      return mapImpl(mapper, builder<B>());
    }

    protected O mapImpl<B, O>(Fn<A, B> mapper, ObserverBuilder<B, O> builder) {
      return builder(obs => subscribe(val => obs.push(mapper(val))));
    }

    public IObservable<B> flatMap<B>(Fn<A, IEnumerable<B>> mapper) {
      return flatMapImpl(mapper, builder<B>());
    }

    public O flatMapImpl<B, O>
    (Fn<A, IEnumerable<B>> mapper, ObserverBuilder<B, O> builder) {
      return builder(obs => subscribe(val => mapper(val).each(obs.push)));
    }

    public IObservable<A> filter(Fn<A, bool> predicate) {
      return filterImpl(predicate, builder<A>());
    }

    protected O filterImpl<O>
    (Fn<A, bool> predicate, ObserverBuilder<A, O> builder) {
      return builder(obs => subscribe(val => {
        if (predicate(val)) obs.push(val);
      }));
    }

    public IObservable<ILinkedList<A>> buffer(int size) {
      return bufferImpl(size, builder<ILinkedList<A>>());
    }

    protected O bufferImpl<O>
    (int size, ObserverBuilder<ILinkedList<A>, O> builder) {
      return builder(obs => {
        var buffer = new LinkedList<A>();
        var roFacade = new ReadOnlyLinkedList<A>(buffer);
        subscribe(val => {
          buffer.AddLast(val);
          if (buffer.Count > size) buffer.RemoveFirst();
          obs.push(roFacade);
        });
      });
    }

    public IObservable<A> join<B>(IObservable<B> other) where B : A {
      return joinImpl(other, builder<A>());
    }

    protected O joinImpl<B, O>
    (IObservable<B> other, ObserverBuilder<A, O> builder) where B : A {
      return builder(obs => {
        subscribe(obs.push);
        other.subscribe(v => obs.push(v));
      });
    }

    public IObservable<A> onceEvery(float seconds) {
      return onceEveryImpl(seconds, builder<A>());
    }

    protected O onceEveryImpl<O>
    (float seconds, ObserverBuilder<A, O> builder) {
      return builder(obs => {
        var lastEmit = float.NegativeInfinity;
        subscribe(value => {
          if (lastEmit + seconds > Time.time) return;
          lastEmit = Time.time;
          obs.push(value);
        });
      });
    }

    public IObservable<ILinkedList<Tpl<A, float>>> 
    withinTimeframe(int count, float timeframe) {
      return withinTimeframeImpl(
        count, timeframe, builder <ILinkedList<Tpl<A, float>>>()
      );
    }

    protected O withinTimeframeImpl<O>(
      int count, float timeframe, 
      ObserverBuilder<ILinkedList<Tpl<A, float>>, O> builder
    ) {
      return builder(obs => 
        map(value => F.t(value, Time.time)).
        buffer(count).
        filter(events => {
          if (events.Count != count) return false;
          var last = events.Last.Value._2;
          return events.All(t => last - t._2 <= timeframe);
        }).subscribe(obs.push)
      );
    }

    public IObservable<Tpl<A, B>> zip<B>(IObservable<B> other) {
      return zipImpl(other, builder<Tpl<A, B>>());
    }

    protected O zipImpl<B, O>
    (IObservable<B> other, ObserverBuilder<Tpl<A, B>, O> builder) {
      return builder(obs => {
        var lastSelf = F.none<A>();
        var lastOther = F.none<B>();
        Action notify = () => lastSelf.each(aVal => lastOther.each(bVal =>
          obs.push(F.t(aVal, bVal))
        ));
        subscribe(val => {
          lastSelf = F.some(val);
          notify();
        });
        other.subscribe(val => {
          lastOther = F.some(val);
          notify();
        });
      });
    }

    public IObservable<Tpl<Option<A>, A>> changesOpt() {
      return changesOptImpl(builder<Tpl<Option<A>, A>>());
    }

    private O changesBase<Elem, O>(
      Act<IObserver<Elem>, Option<A>, A> action, ObserverBuilder<Elem, O> builder
    ) {
      return builder(obs => {
        var lastValue = F.none<A>();
        subscribe(val => {
          action(obs, lastValue, val);
          lastValue = F.some(val);
        });
      });
    }

    protected O changesOptImpl<O>(ObserverBuilder<Tpl<Option<A>, A>, O> builder) {
      return changesBase((obs, lastValue, val) => {
        var valueChanged = lastValue.fold(
          () => true,
          lastVal => EqualityComparer<A>.Default.Equals(lastVal, val)
          );
        if (valueChanged) obs.push(F.t(lastValue, val));
      }, builder);
    }

    public IObservable<Tpl<A, A>> changes() {
      return changesImpl(builder<Tpl<A, A>>());
    }

    protected O changesImpl<O>(ObserverBuilder<Tpl<A, A>, O> builder) {
      return changesBase((obs, lastValue, val) => lastValue.each(lastVal => {
        if (! EqualityComparer<A>.Default.Equals(lastVal, val))
          obs.push(F.t(lastVal, val));
      }), builder);
    }

    public IObservable<A> changedValues() {
      return changedValuesImpl(builder<A>());
    }

    protected O changedValuesImpl<O>(ObserverBuilder<A, O> builder) {
      return changesBase((obs, lastValue, val) => lastValue.voidFold(
        () => obs.push(val),
        lastVal => {
          if (! EqualityComparer<A>.Default.Equals(lastVal, val))
            obs.push(val);
        }
      ), builder);
    }

    private void unsubscribe(Subscription s) {
      subscriptions.IndexWhere(t => t._1 == s).each(subscriptions.RemoveAt);
    }
  }
}