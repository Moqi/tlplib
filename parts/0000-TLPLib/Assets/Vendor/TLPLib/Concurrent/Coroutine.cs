using System.Collections;
using UECoroutine = UnityEngine.Coroutine;

namespace com.tinylabproductions.TLPLib.Concurrent {
  public struct Coroutine {
    public readonly UECoroutine backing;
    public readonly IEnumerator enumerator;

    public Coroutine(UECoroutine backing, IEnumerator enumerator) {
      this.backing = backing;
      this.enumerator = enumerator;
    }
  }
}
