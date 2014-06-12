#if UNITY_TEST
using NUnit.Framework;

namespace com.tinylabproductions.TLPLib.Extensions {
  [TestFixture]
  public class ArrayExtsTest {
    [Test]
    public void ConcatTest1() {
      var a = new[] {1, 2, 3};
      var b = new[] {2, 3, 4};
      var c = new[] {10, 11, 12, 13};
      var d = new[] {9, 8};

      Assert.AreEqual(
        new[] { 1, 2, 3, 2, 3, 4, 10, 11, 12, 13, 9, 8 }, 
        a.concat(b, c, d)
      );
      Assert.AreEqual(
        new[] { 2, 3, 4, 1, 2, 3, 10, 11, 12, 13, 9, 8 },
        b.concat(a, c, d)
      );
    }
  }
}
#endif