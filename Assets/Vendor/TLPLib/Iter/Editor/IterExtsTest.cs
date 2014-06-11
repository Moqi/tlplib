#if UNITY_TEST
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Iter;
using NUnit.Framework;

public class IterExtsTest {
  readonly List<int> list = F.list(1, 2, 3, 4);
  readonly LinkedList<int> linked = F.linkedList(1, 2, 3, 4);
  readonly int elem = 3;

  #region IList

  [Test]
  public void listIterTest() {
    var actual = new List<int>();
    var i = list.iter();
    Assert.AreEqual(F.some(list.Count), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(list, actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void listReverseIterTest() {
    var actual = new List<int>();
    var i = list.rIter();
    Assert.AreEqual(F.some(list.Count), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(list.reversed(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void listSkippedIterTest() {
    var actual = new List<int>();
    var i = list.iter();
    Assert.AreEqual(F.some(list.Count), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(list.Count - 1), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(list.Count - 2), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(list.Skip(2).ToList(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void listSkippedReverseIterTest() {
    var actual = new List<int>();
    var i = list.rIter();
    Assert.AreEqual(F.some(list.Count), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(list.Count - 1), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(list.Count - 2), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(Enumerable.Reverse(list).Skip(2).ToList(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

#endregion

  #region LinkedList

  [Test]
  public void linkedIterTest() {
    var actual = new List<int>();
    var i = linked.iter();
    Assert.AreEqual(F.some(linked.Count), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(linked, actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void linkedReverseIterTest() {
    var actual = new List<int>();
    var i = linked.rIter();
    Assert.AreEqual(F.some(linked.Count), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(linked.Reverse(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void linkedSkippedIterTest() {
    var actual = new List<int>();
    var i = linked.iter();
    Assert.AreEqual(F.some(linked.Count), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(linked.Count - 1), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(linked.Count - 2), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(linked.Skip(2).ToList(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void linkedSkippedReverseIterTest() {
    var actual = new List<int>();
    var i = linked.rIter();
    Assert.AreEqual(F.some(linked.Count), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(linked.Count - 1), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(linked.Count - 2), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(linked.Reverse().Skip(2).ToList(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  #endregion

  #region IEnumerable

  [Test]
  public void enumerableIterTest() {
    var actual = new List<int>();
    var i = list.hIter();
    Assert.AreEqual(F.none<int>(), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(list, actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void enumerableSkippedIterTest() {
    var actual = new List<int>();
    var i = list.hIter();
    Assert.AreEqual(F.none<int>(), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.none<int>(), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.none<int>(), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(list.Skip(2).ToList(), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  #endregion

  #region Single Element

  [Test]
  public void singleElementIterTest() {
    var actual = new List<int>();
    var i = elem.singleIter();
    Assert.AreEqual(F.some(1), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.AreEqual(F.list(elem), actual);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  [Test]
  public void singleElementSkippedIterTest() {
    var actual = new List<int>();
    var i = elem.singleIter();
    Assert.AreEqual(F.some(1), i.elementsLeft);
    i.skip();
    Assert.AreEqual(F.some(0), i.elementsLeft);
    for (; i; i++) actual.Add(~i);
    Assert.True(actual.Count == 0);
    Assert.AreEqual(F.some(0), i.elementsLeft);
  }

  #endregion
}
#endif