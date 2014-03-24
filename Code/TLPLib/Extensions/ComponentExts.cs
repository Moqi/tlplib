using UnityEngine;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class ComponentExts {
    public static A clone<A>(this A self) where A : Component {
      var cloned = (GameObject) Object.Instantiate(self.gameObject);
      return cloned.GetComponent<A>();
    }

    public static A clone<A>(
      this A self, Vector3 position, Quaternion rotation
    ) where A : Component {
      var cloned = (GameObject) Object.
        Instantiate(self.gameObject, position, rotation);
      return cloned.GetComponent<A>();
    }
  }
}
