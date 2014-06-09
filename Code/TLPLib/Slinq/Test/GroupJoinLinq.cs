using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.tinylabproductions.TLPLib.Functional;

using Smooth.Slinq;

#if !UNITY_3_5
namespace Smooth.Slinq.Test {
#endif
	
	public class GroupJoinLinq : MonoBehaviour {
		private void Update() {
			for (int i = 0; i < SlinqTest.loops; ++i) {
				SlinqTest.Tpls1.GroupJoin(SlinqTest.Tpls2, SlinqTest.to_1f, SlinqTest.to_1f, (a, bs) => bs.Count()).Count();
			}
		}
	}

#if !UNITY_3_5
}
#endif
