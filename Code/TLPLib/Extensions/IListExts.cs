using System;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class IListExts {
    public static Option<T> get<T>(this IList<T> list, int index) {
      return (index >= 0 && index < list.Count) 
        ? F.some(list[index]) : F.none<T>();
    }

    public static T updateOrAdd<T>(
      this IList<T> list, Fn<T, bool> finder, Fn<T> ifNotFound, Fn<T, T> ifFound
    ) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug.
      return list.FindWithIndex(finder).fold<T>(
        () => {
          var item = ifNotFound();
          list.Add(item);
          return item;
        },
        t => {
          var updated = ifFound(t._1);
          list[t._2] = updated;
          return updated;
        }
      );
    }

    public static void updateWhere<T>(
      this IList<T> list, Fn<T, bool> finder, Fn<T, T> ifFound
    ) {
      list.FindWithIndex(finder).each(
        t => {
          var updated = ifFound(t._1);
          list[t._2] = updated;
        }
      );
    }

    public static void Shuffle<A>(this IList<A> list) {
      var rng = new Random();
      var n = list.Count;
      while (n > 1) {
        n--;
        var k = rng.Next(n + 1);
        var value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }

    public static A random<A>(this IList<A> list) {
      return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void swap<A>(this IList<A> list, int aIndex, int bIndex) {
      var temp = list[aIndex];
      list[aIndex] = list[bIndex];
      list[bIndex] = temp;
    }
  }
}
