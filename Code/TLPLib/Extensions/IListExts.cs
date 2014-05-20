using System;
using System.Collections.Generic;
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

    public static T updateOrAdd<T>(
      this IList<T> list, Fn<T, bool> finder, Fn<T> ifNotFound, Fn<T, T> ifFound
    ) {
      return list.findWithIndex(finder).fold(
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
      list.findWithIndex(finder).each(t => {
        var updated = ifFound(t._1);
        list[t._2] = updated;
      });
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

    public static IList<B> map<A, B>(this IList<A> list, Fn<A, B> mapper) {
      var nList = new List<B>(list.Count);
      list.each(e => nList.Add(mapper(e)));
      return nList;
    }

    public static IList<B> mapWithIndex<A, B>(this IList<A> list, Fn<A, int, B> mapper) {
      var nList = new List<B>(list.Count);
      list.eachWithIndex((e, i) => nList.Add(mapper(e, i)));
      return nList;
    }

    public static IList<A> filter<A>(this IList<A> list, Fn<A, bool> predicate) {
      var nList = new List<A>(list.Count);
      list.each(e => { if (predicate(e)) nList.Add(e); });
      return nList;
    }

    public static A random<A>(this IList<A> list) {
      return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void swap<A>(this IList<A> list, int aIndex, int bIndex) {
      var temp = list[aIndex];
      list[aIndex] = list[bIndex];
      list[bIndex] = temp;
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
