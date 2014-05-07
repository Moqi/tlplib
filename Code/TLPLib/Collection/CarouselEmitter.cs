using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Functional;

namespace com.tinylabproductions.TLPLib.Collection {
  /**
   * Carousel like item emitter.
   * 
   * Given:
   * * priorities: ["a", "c", "b"]
   * * counts: {"a" => 3, "b" => 2, "c" => 1}
   * 
   * Will yield:
   * "a", "c", "b", "a", "b", "a", starts from beggining.
   * 
   * This enumerable is endless.
   **/
  public class CarouselEmitter<A> : IEnumerable<A> {
    protected readonly Tpl<A, int>[] counts;

    public CarouselEmitter(
      IEnumerable<A> priorities, IDictionary<A, int> counts
    ) {
      this.counts = priorities.Select(a =>
        F.t(a, counts.get(a).getOrElse(() => 0))
      ).Where(t => t._2 > 0).ToArray();
    }

    public int itemCount { get { return counts.Length; } }

    public IEnumerator<A> GetEnumerator() {
      var maxShows = counts.Max(t => t._2);
      if (maxShows <= 0) yield break;

      while (true) {
        for (var idx = 0; idx < maxShows; idx++) {
          foreach (var t in counts.Where(t => t._2 > idx))
            yield return t._1;
        }
      }
    }

    /** Returns an enumerator with N elements skipped. **/
    public IEnumerator<A> GetEnumerator(int skippedElements) {
      var enumerator = GetEnumerator();
      for (var i = 0; i < skippedElements; i++) enumerator.MoveNext();
      return enumerator;
    }

    /** Returns an enumerator which has (0, itemCount] elements skipped. **/
    public IEnumerator<A> GetRandomSkipEnumerator() {
      return GetEnumerator((new Random()).Next(0, itemCount));
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
