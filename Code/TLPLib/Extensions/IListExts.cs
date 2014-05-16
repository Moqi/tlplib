using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Extensions {
  public static class IListExts {
    public static Option<T> get<T>(this IList<T> list, int index) {
      return (index >= 0 && index < list.Count) 
        ? F.some(list[index]) : F.none<T>();
    }

    // AOT safe version of ToDictionary.
    public static IDictionary<K, V> toDict<A, K, V>(
      this IList<A> list, Fn<A, K> keyGetter, Fn<A, V> valueGetter
    ) {
      var dict = new Dictionary<K, V>();
      // ReSharper disable once LoopCanBeConvertedToQuery
      // We're trying to avoid LINQ to avoid iOS AOT related issues.
      foreach (var item in list) 
        dict.Add(keyGetter(item), valueGetter(item));
      return dict;
    }

    public static IList<B> mapWithIndex<A, B>(
      this IList<A> list, Fn<A, int, B> mapper
    ) {
      var nList = new List<B>(list.Count);
      for (var idx = 0; idx < list.Count; idx++)
        nList.Add(mapper(list[idx], idx));
      return nList;
    }

    public static T updateOrAdd<T>(
      this IList<T> list, Fn<T, bool> finder, Fn<T> ifNotFound, Fn<T, T> ifFound
    ) {
      return list.FindWithIndex(finder).fold(
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

    public static Option<A> headOpt<A>(this IList<A> list) {
      return list.Count == 0 ? F.none<A>() : F.some(list[0]);
    }

    public static Option<A> lastOpt<A>(this IList<A> list) {
      return list.Count == 0 ? F.none<A>() : F.some(list[list.Count - 1]);
    }

    public static IEnumerable<A> dropRight<A>(this IList<A> list, int count) {
      var end = list.Count - count;
      var idx = 0;
      foreach (var item in list) {
        if (idx < end) yield return item;
        idx++;
      }
    }
  }
}
