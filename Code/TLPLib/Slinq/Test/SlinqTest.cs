using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.tinylabproductions.TLPLib.Functional;

#if !UNITY_3_5
namespace Smooth.Slinq.Test {
#endif

	/// <summary>
	/// Test controller and (de)verifier for Smooth.Slinq.
	/// </summary>
	public class SlinqTest : MonoBehaviour {
		public static readonly Fn<Tpl<int, int>, Tpl<int, int>, bool> eq = (a, b) => a == b;
		public static readonly Fn<Tpl<int, int, int>, Tpl<int, int, int>, bool> eq_t3 = (a, b) => a == b;

		public static readonly Fn<Tpl<int, int>, int> to_1 = t => t._1;
		public static readonly Func<Tpl<int, int>, int> to_1f = t => t._1;

		public static readonly IEqualityComparer<Tpl<int, int>> eq_1 = new Equals_1<int, int>();
		//public static readonly Fn<Tpl<int, int>, Tpl<int, int>, bool> eq_1_p = Comparisons<Tpl<int, int>>.ToPredicate(eq_1);

		public static readonly StringBuilder stringBuilder = new StringBuilder();

		public static List<Tpl<int, int>> Tpls1 = new List<Tpl<int, int>>();
		public static List<Tpl<int, int>> Tpls2 = new List<Tpl<int, int>>();

		public static int loops = 1;

		public int minCount = 50;
		public int maxCount = 100;

		public int minValue = 1;
		public int maxValue = 100;

		public int speedLoops = 10;
	
		public bool testCorrectness;

		private void Start() {
			Tpls1 = new List<Tpl<int, int>>(maxCount);
			Tpls2 = new List<Tpl<int, int>>(maxCount);
			loops = speedLoops;

			Debug.Log("Element count: " + minCount + " to " + maxCount + ", value range: " + minValue + " to " + maxValue + ", loops: " + loops);
			
			if (testCorrectness) {
				Debug.Log("Testing Correctness.");
			}
		}

		private void Update() {
			if (testCorrectness) {
				TestCorrectness();
			}
		}

		private void LateUpdate() {
			loops = speedLoops;

			Tpls1.Clear();
			Tpls2.Clear();

			var count = UnityEngine.Random.Range(minCount, maxCount + 1);
			for (int i = 0; i < count; ++i) {
				Tpls1.Add(new Tpl<int, int>(UnityEngine.Random.Range(minValue, maxValue + 1), i));
				Tpls2.Add(new Tpl<int, int>(UnityEngine.Random.Range(minValue, maxValue + 1), i));
			}
		}

		private void TestCorrectness() {
			var testTpl = Tpls2.Slinq().FirstOrDefault();
			var testInt = testTpl._1;
			var testInt2 = testInt * (maxValue - minValue + 1) / 25;
			var midSkip = UnityEngine.Random.value < 0.5f ? testInt : 0;

			if (Tpls1.Slinq().Aggregate(0, (acc, next) => acc + next._1) != Tpls1.Aggregate(0, (acc, next) => acc + next._1)) {
				Debug.LogError("Aggregate failed.");
				testCorrectness = false;
			}

			if (Tpls1.Slinq().Aggregate(0, (acc, next) => acc + next._1, acc => -acc) != Tpls1.Aggregate(0, (acc, next) => acc + next._1, acc => -acc)) {
				Debug.LogError("Aggregate failed.");
				testCorrectness = false;
			}
			
			if (Tpls1.Slinq().AggregateWhile(0, (acc, next) => acc < testInt2 ? new Option<int>(acc + next._1) : new Option<int>()) != Tpls1.Slinq().AggregateRunning(0, (acc, next) => acc + next._1).Where(acc => acc >= testInt2).FirstOrDefault()) {
				Debug.LogError("AggregateWhile / AggregateRunning failed.");
				testCorrectness = false;
			}

			if (Tpls1.Slinq().All(x => x._1 < testInt2) ^ Tpls1.All(x => x._1 < testInt2)) {
				Debug.LogError("All failed.");
				testCorrectness = false;
			}

			if (Tpls1.Slinq().All((x, c) => x._1 < c, testInt2) ^ Tpls1.All(x => x._1 < testInt2)) {
				Debug.LogError("All failed.");
				testCorrectness = false;
			}

			if (Tpls1.Slinq().Any(x => x._1 > testInt2) ^ Tpls1.Any(x => x._1 > testInt2)) {
				Debug.LogError("All failed.");
				testCorrectness = false;
			}
			
			if (Tpls1.Slinq().Any((x, c) => x._1 > c, testInt2) ^ Tpls1.Any(x => x._1 > testInt2)) {
				Debug.LogError("All failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Select(to_1).AverageOrNone().cata(avg => avg == Tpls1.Select(to_1f).Average(), Tpls1.Count == 0)) {
				Debug.LogError("AverageOrNone failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Concat(Tpls2.Slinq()).SequenceEqual(Tpls1.Concat(Tpls2).Slinq(), eq)) {
				Debug.LogError("Concat failed.");
				testCorrectness = false;
			}
			
			if (Tpls1.Slinq().Contains(testTpl, eq_1) ^ Tpls1.Contains(testTpl, eq_1)) {
				Debug.LogError("Contains failed.");
				testCorrectness = false;
			}
			
			if (Tpls1.Slinq().Where(x => x._1 < testInt).Count() != Tpls1.Where(x => x._1 < testInt).Count()) {
				Debug.LogError("Count failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Distinct(eq_1).SequenceEqual(Tpls1.Distinct(eq_1).Slinq(), eq)) {
				Debug.LogError("Distinct failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Except(Tpls2.Slinq(), eq_1).SequenceEqual(Tpls1.Except(Tpls2, eq_1).Slinq(), eq)) {
				Debug.LogError("Except failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().FirstOrNone().cata(x => x == Tpls1.First(), !Tpls1.Any())) {
				Debug.LogError("FirstOrNone failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().FirstOrNone(x => x._1 < testInt).cata(x => x == Tpls1.First(z => z._1 < testInt), !Tpls1.Where(z => z._1 < testInt).Any())) {
				Debug.LogError("FirstOrNone(predicate) failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().FirstOrNone((x, t) => x._1 < t, testInt).cata(x => x == Tpls1.First(z => z._1 < testInt), !Tpls1.Where(z => z._1 < testInt).Any())) {
				Debug.LogError("FirstOrNone(predicate, parameter) failed.");
				testCorrectness = false;
			}
			
			if (!new List<Tpl<int, int>>[] { Tpls1, Tpls2 }.Slinq().Select(x => x.Slinq()).Flatten().SequenceEqual(Tpls1.Concat(Tpls2).Slinq(), eq)) {
				Debug.LogError("Flatten failed.");
				testCorrectness = false;
			}

			{
				var feAcc = 0;
				Tpls1.Slinq().ForEach(x => feAcc += x._1);
				if (feAcc != Tpls1.Slinq().Select(to_1).Sum()) {
					Debug.LogError("ForEach failed.");
					testCorrectness = false;
				}
			}

			if (!Tpls1.Slinq().Intersect(Tpls2.Slinq(), eq_1).SequenceEqual(Tpls1.Intersect(Tpls2, eq_1).Slinq(), eq)) {
				Debug.LogError("Intersect failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().GroupBy(to_1).SequenceEqual(
				Tpls1.GroupBy(to_1f).Slinq(),
				(s, l) => s.key == l.Key && s.values.SequenceEqual(l.Slinq(), eq))) {
				Debug.LogError("GroupBy failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().GroupJoin(Tpls2.Slinq(), to_1, to_1, (a, bs) => F.t(a._1, a._2, bs.Count()))
			    .SequenceEqual(Tpls1.GroupJoin(Tpls2, to_1f, to_1f, (a, bs) => F.t(a._1, a._2, bs.Count())).Slinq(), eq_t3)) {
				Debug.LogError("GroupJoin failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Join(Tpls2.Slinq(), to_1, to_1, (a, b) => F.t(a._1, a._2, b._2))
			    .SequenceEqual(Tpls1.Join(Tpls2, to_1f, to_1f, (a, b) => F.t(a._1, a._2, b._2)).Slinq(), eq_t3)) {
				Debug.LogError("Join failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().LastOrNone().cata(x => x == Tpls1.Last(), !Tpls1.Any())) {
				Debug.LogError("LastOrNone failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().LastOrNone(x => x._1 < testInt).cata(x => x == Tpls1.Last(z => z._1 < testInt), !Tpls1.Where(z => z._1 < testInt).Any())) {
				Debug.LogError("LastOrNone(predicate) failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().LastOrNone((x, t) => x._1 < t, testInt).cata(x => x == Tpls1.Last(z => z._1 < testInt), !Tpls1.Where(z => z._1 < testInt).Any())) {
				Debug.LogError("LastOrNone(predicate, parameter) failed.");
				testCorrectness = false;
			}
			
			if (Tpls1.Count > 0 && Tpls1.Slinq().Max() != Tpls1.Max()) {
				Debug.LogError("Max failed.");
				testCorrectness = false;
			}

			if (Tpls1.Count > 0 && Tpls1.Slinq().Min() != Tpls1.Min()) {
				Debug.LogError("Min failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().MaxOrNone().cata(x => x == Tpls1.Max(), Tpls1.Count == 0)) {
				Debug.LogError("MaxOrNone failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().MinOrNone().cata(x => x == Tpls1.Min(), Tpls1.Count == 0)) {
				Debug.LogError("MinOrNone failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().OrderBy(to_1).SequenceEqual(Tpls1.OrderBy(to_1f).Slinq(), eq)) {
				Debug.LogError("OrderBy failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().OrderByDescending(to_1).SequenceEqual(Tpls1.OrderByDescending(to_1f).Slinq(), eq)) {
				Debug.LogError("OrderByDescending failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().OrderBy().SequenceEqual(Tpls1.OrderBy(x => x).Slinq(), eq)) {
				Debug.LogError("OrderBy keyless failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().OrderByDescending().SequenceEqual(Tpls1.OrderByDescending(x => x).Slinq(), eq)) {
				Debug.LogError("OrderByDescending keyless failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().OrderByGroup(to_1).SequenceEqual(Tpls1.OrderBy(to_1f).Slinq(), eq)) {
				Debug.LogError("OrderByGroup failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().OrderByGroupDescending(to_1).SequenceEqual(Tpls1.OrderByDescending(to_1f).Slinq(), eq)) {
				Debug.LogError("OrderByGroupDescending failed.");
				testCorrectness = false;
			}
			
			{
				var list = RemovableList();
				var slinq = list.Slinq().Skip(midSkip);
				for (int i = 0; i < testInt; ++i) {
					slinq.Remove();
				}
				if (!list.Slinq().Skip(midSkip).SequenceEqual(Tpls1.Slinq().Skip(midSkip).Skip(testInt), eq)) {
					Debug.LogError("Remove failed.");
					testCorrectness = false;
				}
			}
			
			{
				var list = RemovableList();
				var slinq = list.SlinqDescending().Skip(midSkip);
				for (int i = 0; i < testInt; ++i) {
					slinq.Remove();
				}
				if (!list.SlinqDescending().Skip(midSkip).SequenceEqual(Tpls1.SlinqDescending().Skip(midSkip).Skip(testInt), eq)) {
					Debug.LogError("Remove descending failed.");
					testCorrectness = false;
				}
			}

			{
				var list = RemovableList();
				list.Slinq().Skip(midSkip).Remove(testInt);
				if (!list.Slinq().Skip(midSkip).SequenceEqual(Tpls1.Slinq().Skip(midSkip).Skip(testInt), eq)) {
					Debug.LogError("Remove(int) failed.");
					testCorrectness = false;
				}
			}
			
			{
				var list = RemovableList();
				list.Slinq().Skip(midSkip).RemoveWhile(x => x._1 < testInt2);
				if (!list.Slinq().Skip(midSkip).SequenceEqual(Tpls1.Slinq().Skip(midSkip).SkipWhile(x => x._1 < testInt2), eq)) {
					Debug.LogError("RemoveWhile failed.");
					testCorrectness = false;
				}
			}

			{
				var list = RemovableList();
				var sSlinq = Tpls1.Slinq().Skip(midSkip);
				var rAcc = list.Slinq().Skip(midSkip).RemoveWhile(0, (acc, next) => acc < testInt2 ? new Option<int>(acc + next._1) : new Option<int>());
				var sAcc = sSlinq.SkipWhile(0, (acc, next) => acc < testInt2 ? new Option<int>(acc + next._1) : new Option<int>());

				if (rAcc != sAcc || !list.Slinq().Skip(midSkip).SequenceEqual(sSlinq, eq)) {
					Debug.LogError("RemoveWhile aggregating failed.");
					testCorrectness = false;
				}
			}
			
			
			{
				var list = RemovableLinkedList();
				var slinq = list.Slinq().Skip(midSkip);
				for (int i = 0; i < testInt; ++i) {
					slinq.Remove();
				}
				if (!list.Slinq().Skip(midSkip).SequenceEqual(Tpls1.Slinq().Skip(midSkip).Skip(testInt), eq)) {
					Debug.LogError("Remove LL failed.");
					testCorrectness = false;
				}
			}
			
			
			{
				var list = RemovableLinkedList();
				var slinq = list.SlinqDescending().Skip(midSkip);
				for (int i = 0; i < testInt; ++i) {
					slinq.Remove();
				}
				if (!list.SlinqDescending().Skip(midSkip).SequenceEqual(Tpls1.SlinqDescending().Skip(midSkip).Skip(testInt), eq)) {
					Debug.LogError("Remove descending LL failed.");
					testCorrectness = false;
				}
			}
			
			{
				var list = RemovableLinkedList();
				list.Slinq().Skip(midSkip).Remove(testInt);
				if (!list.Slinq().Skip(midSkip).SequenceEqual(Tpls1.Slinq().Skip(midSkip).Skip(testInt), eq)) {
					Debug.LogError("Remove(int) LL failed.");
					testCorrectness = false;
				}
			}
			
			{
				var list = RemovableLinkedList();
				list.Slinq().Skip(midSkip).RemoveWhile(x => x._1 < testInt2);
				if (!list.Slinq().Skip(midSkip).SequenceEqual(Tpls1.Slinq().Skip(midSkip).SkipWhile(x => x._1 < testInt2), eq)) {
					Debug.LogError("RemoveWhile LL failed.");
					testCorrectness = false;
				}
			}
			
			{
				var list = RemovableLinkedList();
				var sSlinq = Tpls1.Slinq().Skip(midSkip);
				var rAcc = list.Slinq().Skip(midSkip).RemoveWhile(0, (acc, next) => acc < testInt2 ? new Option<int>(acc + next._1) : new Option<int>());
				var sAcc = sSlinq.SkipWhile(0, (acc, next) => acc < testInt2 ? new Option<int>(acc + next._1) : new Option<int>());
				
				if (rAcc != sAcc || !list.Slinq().Skip(midSkip).SequenceEqual(sSlinq, eq)) {
					Debug.LogError("RemoveWhile aggregating LL failed.");
					testCorrectness = false;
				}
			}

			if (!Tpls1.Slinq().Reverse().SequenceEqual(Enumerable.Reverse(Tpls1).Slinq(), eq)) {
				Debug.LogError("Reverse failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Select(to_1).SequenceEqual(Tpls1.Select(to_1f).Slinq())) {
				Debug.LogError("Select failed.");
				testCorrectness = false;
			}
			
			if (!new List<Tpl<int, int>>[] { Tpls1, Tpls2 }.Slinq().SelectMany(x => x.Slinq()).SequenceEqual(Tpls1.Concat(Tpls2).Slinq(), eq)) {
				Debug.LogError("SelectMany failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().SelectMany(x => x._1 < testInt ? new Option<int>(x._1) : new Option<int>()).SequenceEqual(Tpls1.Where(x => x._1 < testInt).Select(x => x._1).Slinq())) {
				Debug.LogError("SelectMany option failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().SequenceEqual(Tpls1.Slinq(), eq)) {
				Debug.LogError("SequenceEqual failed.");
				testCorrectness = false;
			}
			
			if (Tpls1.Slinq().SequenceEqual(Tpls2.Slinq(), eq) ^ Tpls1.SequenceEqual(Tpls2)) {
				Debug.LogError("SequenceEqual failed.");
				testCorrectness = false;
			}
			
			{
				var a = new int[3];
				a[2] = testInt;
				if (a.Slinq().SingleOrNone().isSome ||
				    a.Slinq().Skip(1).SingleOrNone().isSome ||
				    !a.Slinq().Skip(2).SingleOrNone().exists(testInt) ||
				    a.Slinq().Skip(3).SingleOrNone().isSome) {
					Debug.LogError("SingleOrNone failed.");
					testCorrectness = false;
				}
			}

			{
				var slinq = Tpls1.Slinq();
				for (int i = 0; i < testInt; ++i) {
					slinq.Skip();
				}
				if (!Tpls1.Slinq().Skip(testInt).SequenceEqual(slinq, eq)) {
					Debug.LogError("Skip failed.");
					testCorrectness = false;
				}
			}
			
			if (!Tpls1.Slinq().SkipWhile(x => x._1 < testInt2).SequenceEqual(Tpls1.SkipWhile(x => x._1 < testInt2).Slinq(), eq)) {
				Debug.LogError("SkipWhile failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Reverse().SequenceEqual(Tpls1.SlinqDescending(), eq)) {
				Debug.LogError("SlinqDescending failed.");
				testCorrectness = false;
			}

			if (!Tpls1.SlinqDescending().SequenceEqual(RemovableLinkedList().SlinqDescending(), eq)) {
				Debug.LogError("SlinqDescending LL failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().SequenceEqual(RemovableLinkedList().SlinqNodes().Select(x => x.Value), eq)) {
				Debug.LogError("SlinqNodes LL failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.SlinqDescending().SequenceEqual(RemovableLinkedList().SlinqNodesDescending().Select(x => x.Value), eq)) {
				Debug.LogError("SlinqNodesDescending LL failed.");
				testCorrectness = false;
			}

			if (!Tpls1.SlinqWithIndex().All(x => x._1._2 == x._2) ||
			    !Tpls1.SlinqWithIndex().Select(x => x._1).SequenceEqual(Tpls1.Slinq(), eq)) {
				Debug.LogError("SlinqWithIndex failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.SlinqWithIndexDescending().All(x => x._1._2 == x._2) ||
			    !Tpls1.SlinqWithIndexDescending().Select(x => x._1).SequenceEqual(Tpls1.SlinqDescending(), eq)) {
				Debug.LogError("SlinqWithIndexDescending failed.");
				testCorrectness = false;
			}

			if (Tpls1.Slinq().Select(to_1).Sum() != Tpls1.Select(to_1f).Sum()) {
				Debug.LogError("Sum failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Take(testInt).SequenceEqual(Tpls1.Take(testInt).Slinq(), eq)) {
				Debug.LogError("Take failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().TakeRight(testInt).SequenceEqual(Tpls1.Slinq().Skip(Tpls1.Count - testInt), eq)) {
				Debug.LogError("TakeRight failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().TakeWhile(x => x._1 < testInt2).SequenceEqual(Tpls1.TakeWhile(x => x._1 < testInt2).Slinq(), eq)) {
				Debug.LogError("TakeWhile failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().TakeWhile((x, c) => x._1 < c, testInt2).SequenceEqual(Tpls1.TakeWhile(x => x._1 < testInt2).Slinq(), eq)) {
				Debug.LogError("TakeWhile failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Union(Tpls2.Slinq(), eq_1).SequenceEqual(Tpls1.Union(Tpls2, eq_1).Slinq(), eq)) {
				Debug.LogError("Union failed.");
				testCorrectness = false;
			}
	
			if (!Tpls1.Slinq().Where(x => x._1 < testInt).SequenceEqual(Tpls1.Where(x => x._1 < testInt).Slinq(), eq)) {
				Debug.LogError("Where failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Where((x, c) => x._1 < c, testInt).SequenceEqual(Tpls1.Where(x => x._1 < testInt).Slinq(), eq)) {
				Debug.LogError("Where failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Zip(Tpls2.Slinq()).SequenceEqual(
				Slinqable.Sequence(0, 1)
				.TakeWhile(x => x < Tpls1.Count && x < Tpls2.Count)
				.Select(x => F.t(Tpls1[x], Tpls2[x])))) {
				Debug.LogError("Zip Tpls failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().Zip(Tpls2.Slinq(), (a, b) => F.t(a._1, b._1)).SequenceEqual(
				Slinqable.Sequence(0, 1)
				.TakeWhile(x => x < Tpls1.Count && x < Tpls2.Count)
				.Select(x => F.t(Tpls1[x]._1, Tpls2[x]._1)), eq)) {
				Debug.LogError("Zip failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq()
			    .ZipAll(Tpls2.Slinq())
			    .SequenceEqual(Slinqable.Sequence(0, 1)
			               .TakeWhile(x => x < Tpls1.Count || x < Tpls2.Count)
			               .Select(x => F.t(
				x < Tpls1.Count ?	new Option<Tpl<int, int>>(Tpls1[x]) : new Option<Tpl<int, int>>(),
				x < Tpls2.Count ?	new Option<Tpl<int, int>>(Tpls2[x]) : new Option<Tpl<int, int>>())))) {
				Debug.LogError("ZipAll Tpls failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Skip(midSkip)
			    .ZipAll(Tpls2.Slinq())
			    .SequenceEqual(Slinqable.Sequence(0, 1)
			               .TakeWhile(x => x + midSkip < Tpls1.Count || x < Tpls2.Count)
			               .Select(x => F.t(
				x + midSkip < Tpls1.Count ? new Option<Tpl<int, int>>(Tpls1[x + midSkip]) : new Option<Tpl<int, int>>(),
				x < Tpls2.Count ? new Option<Tpl<int, int>>(Tpls2[x]) : new Option<Tpl<int, int>>())))) {
				Debug.LogError("ZipAll Tpls failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq()
			    .ZipAll(Tpls2.Slinq().Skip(midSkip))
			    .SequenceEqual(Slinqable.Sequence(0, 1)
			               .TakeWhile(x => x < Tpls1.Count || x + midSkip < Tpls2.Count)
			               .Select(x => F.t(
				x < Tpls1.Count ? new Option<Tpl<int, int>>(Tpls1[x]) : new Option<Tpl<int, int>>(),
				x + midSkip < Tpls2.Count ? new Option<Tpl<int, int>>(Tpls2[x + midSkip]) : new Option<Tpl<int, int>>())))) {
				Debug.LogError("ZipAll Tpls failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq()
			    .ZipAll(Tpls2.Slinq(), (a, b) => F.t(a, b))
			    .SequenceEqual(Slinqable.Sequence(0, 1)
			               .TakeWhile(x => x < Tpls1.Count || x < Tpls2.Count)
			               .Select(x => F.t(
				x < Tpls1.Count ?	new Option<Tpl<int, int>>(Tpls1[x]) : new Option<Tpl<int, int>>(),
				x < Tpls2.Count ?	new Option<Tpl<int, int>>(Tpls2[x]) : new Option<Tpl<int, int>>())))) {
				Debug.LogError("ZipAll failed.");
				testCorrectness = false;
			}
			
			if (!Tpls1.Slinq().Skip(midSkip)
			    .ZipAll(Tpls2.Slinq(), (a, b) => F.t(a, b))
			    .SequenceEqual(Slinqable.Sequence(0, 1)
			               .TakeWhile(x => x + midSkip < Tpls1.Count || x < Tpls2.Count)
			               .Select(x => F.t(
				x + midSkip < Tpls1.Count ? new Option<Tpl<int, int>>(Tpls1[x + midSkip]) : new Option<Tpl<int, int>>(),
				x < Tpls2.Count ? new Option<Tpl<int, int>>(Tpls2[x]) : new Option<Tpl<int, int>>())))) {
				Debug.LogError("ZipAll failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq()
			    .ZipAll(Tpls2.Slinq().Skip(midSkip), (a, b) => F.t(a, b))
			    .SequenceEqual(Slinqable.Sequence(0, 1)
			               .TakeWhile(x => x < Tpls1.Count || x + midSkip < Tpls2.Count)
			               .Select(x => F.t(
				x < Tpls1.Count ? new Option<Tpl<int, int>>(Tpls1[x]) : new Option<Tpl<int, int>>(),
				x + midSkip < Tpls2.Count ? new Option<Tpl<int, int>>(Tpls2[x + midSkip]) : new Option<Tpl<int, int>>())))) {
				Debug.LogError("ZipAll failed.");
				testCorrectness = false;
			}

			if (!Tpls1.Slinq().ZipWithIndex().All(x => x._1._2 == x._2) ||
			    !Tpls1.Slinq().ZipWithIndex().Select(x => x._1).SequenceEqual(Tpls1.Slinq(), eq)) {
				Debug.LogError("ZipWithIndex failed.");
				testCorrectness = false;
			}
		}

		private List<Tpl<int, int>> RemovableList() {
			return new List<Tpl<int, int>>(Tpls1);
		}

		private LinkedList<Tpl<int, int>> RemovableLinkedList() {
			return new LinkedList<Tpl<int, int>>(Tpls1);
		}

		public class Equals_1<T1, T2> : IEquatable<Equals_1<T1, T2>>, IEqualityComparer<Tpl<T1, T2>> {
			public readonly IEqualityComparer<T1> EqComparer;

			public Equals_1() {
				this.EqComparer = Smooth.Collections.EqComparer<T1>.Default;
			}

			public Equals_1(IEqualityComparer<T1> EqComparer) {
				this.EqComparer = EqComparer;
			}
			
			public override bool Equals(object o) {
				return o is Equals_1<T1, T2> && this.Equals((Equals_1<T1, T2>) o);
			}
			
			public bool Equals(Equals_1<T1, T2> other) {
				return this.EqComparer == other.EqComparer;
			}
			
			public override int GetHashCode() {
				return this.EqComparer.GetHashCode();
			}
			
			public static bool operator == (Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs) {
				return lhs.Equals(rhs);
			}
			
			public static bool operator != (Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs) {
				return !lhs.Equals(rhs);
			}

			public bool Equals(Tpl<T1, T2> a, Tpl<T1, T2> b) {
				return EqComparer.Equals(a._1, b._1);
			}

			public int GetHashCode(Tpl<T1, T2> a) {
				return EqComparer.GetHashCode(a._1);
			}
		}
	}

#if !UNITY_3_5
}
#endif
