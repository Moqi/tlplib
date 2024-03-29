﻿#if UNITY_TEST
using Castle.Core.Internal;
using System;
using System.Linq;
using com.tinylabproductions.TLPLib.Functional;
using NUnit.Framework;

namespace com.tinylabproductions.TLPLib.Reactive {
  [TestFixture]
  public class IObservableTest {
    [Test]
    public void SubscriptionCounting() {
      var o = Observable.interval(1f);
      var s1 = o.subscribe(_ => {});
      var s2 = o.subscribe(_ => {});
      Assert.AreEqual(2, o.subscribers);
      s1.unsubscribe();
      Assert.AreEqual(1, o.subscribers);
      s1.unsubscribe();
      Assert.AreEqual(1, o.subscribers);
      s2.unsubscribe();
      Assert.AreEqual(0, o.subscribers);
      var s3 = o.subscribe(_ => { });
      Assert.AreEqual(1, o.subscribers);
      s3.unsubscribe();
      Assert.AreEqual(0, o.subscribers);
    }

    [Test]
    public void NestedSubscriptionCounting() {
      var o = Observable.interval(1f);
      var o2 = o.map(_ => 1);
      Assert.AreEqual(0, o.subscribers);
      Assert.AreEqual(0, o2.subscribers);
      var s = o2.subscribe(_ => { });
      Assert.AreEqual(1, o.subscribers);
      Assert.AreEqual(1, o2.subscribers);
      s.unsubscribe();
      Assert.AreEqual(0, o.subscribers);
      Assert.AreEqual(0, o2.subscribers);
    }

    [Test]
    public void SubscribeFromInsideEvent() {
      var subject = new Subject<Unit>();
      var called = 0;
      subject.subscribe(_ => subject.subscribe(__ => called++));
      subject.push(F.unit);
      Assert.AreEqual(1, called);
      subject.push(F.unit);
      Assert.AreEqual(3, called);
    }

    [Test]
    public void UnsubscribeAfterEvent() {
      var subject = new Subject<Unit>();
      var called = 0;
      subject.subscribe((_, subscription) => {
        called += 1;
        subscription.unsubscribe();
      });
      subject.push(F.unit);
      subject.push(F.unit);
      Assert.AreEqual(1, called);
    }

    [Test]
    public void Map() {
      var subj = new Subject<int>();
      var list = F.list<int>();
      subj.map(i => list.Count).subscribe(list.Add);
      Enumerable.Range(5, 5).ForEach(subj.push);
      Assert.AreEqual(F.list(0, 1, 2, 3, 4), list);
    }

    [Test]
    public void FlatMap() {
      var subj = new Subject<int>();
      var list = F.list<int>();
      subj.flatMap(i => Enumerable.Range(0, i)).subscribe(list.Add);
      Enumerable.Range(1, 3).ForEach(subj.push);
      Assert.AreEqual(F.list(0, 0, 1, 0, 1, 2), list);
    }

    [Test]
    public void Filter() {
      var subj = new Subject<int>();
      var list = F.list<int>();
      subj.filter(i => i % 2 == 0).subscribe(list.Add);
      Enumerable.Range(0, 5).ForEach(subj.push);
      Assert.AreEqual(F.list(0, 2, 4), list);
    }

    [Test]
    public void Zip2() {
      var subj1 = new Subject<int>();
      var subj2 = new Subject<int>();
      var list = F.list<Tpl<int, int>>();
      subj1.zip(subj2).subscribe(list.Add);
      subj1.push(1);
      Assert.AreEqual(0, list.Count);
      subj2.push(1);
      Assert.AreEqual(F.list(F.t(1, 1)), list);
      subj2.push(2);
      Assert.AreEqual(F.list(F.t(1, 1), F.t(1, 2)), list);
      subj1.push(0);
      Assert.AreEqual(F.list(F.t(1, 1), F.t(1, 2), F.t(0, 2)), list);
    }

    [Test]
    public void Zip3() {
      var subj1 = new Subject<int>();
      var subj2 = new Subject<int>();
      var subj3 = new Subject<int>();
      var list = F.list<Tpl<int, int, int>>();
      subj1.zip(subj2, subj3).subscribe(list.Add);
      subj1.push(1);
      Assert.AreEqual(0, list.Count);
      subj2.push(1);
      Assert.AreEqual(0, list.Count);
      subj3.push(1);
      Assert.AreEqual(F.list(F.t(1, 1, 1)), list);
      subj2.push(2);
      Assert.AreEqual(F.list(F.t(1, 1, 1), F.t(1, 2, 1)), list);
      subj3.push(0);
      Assert.AreEqual(F.list(F.t(1, 1, 1), F.t(1, 2, 1), F.t(1, 2, 0)), list);
    }
  }
}
#endif