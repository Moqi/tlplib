using com.tinylabproductions.TLPLib.Functional;
using NUnit.Framework;

namespace com.tinylabproductions.TLPLib.Extensions {
  [TestFixture]
  public class IEnumerableExtsTest {
    [Test]
    public void MinMaxMin1() {
      var actual = new[] {1, 2, 3, 4, 5, 6}.minMax((a, b) => a < b);
      Assert.AreEqual(F.some(1), actual);
    }

    [Test]
    public void MinMaxMin2() {
      var actual = new[] {6, 4, 3, 2, 1}.minMax((a, b) => a < b);
      Assert.AreEqual(F.some(1), actual);
    }

    [Test]
    public void MinMaxMin3() {
      var actual = new[] {2,5,6,1,7,2}.minMax((a, b) => a < b);
      Assert.AreEqual(F.some(1), actual);
    }

    [Test]
    public void MinMaxMax1() {
      var actual = new[] {1, 2, 3, 4, 5, 6}.minMax((a, b) => a > b);
      Assert.AreEqual(F.some(6), actual);
    }

    [Test]
    public void MinMaxMax2() {
      var actual = new[] {6, 4, 3, 2, 1}.minMax((a, b) => a > b);
      Assert.AreEqual(F.some(6), actual);
    }

    [Test]
    public void MinMaxMax3() {
      var actual = new[] {2,5,6,1,7,2}.minMax((a, b) => a > b);
      Assert.AreEqual(F.some(7), actual);
    }
  }
}
