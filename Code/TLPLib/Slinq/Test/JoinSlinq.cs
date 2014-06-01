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
	
	public class JoinSlinq : MonoBehaviour {
		private void Update() {
			for (int i = 0; i < SlinqTest.loops; ++i) {
				SlinqTest.Tpls1.Slinq().Join(SlinqTest.Tpls2.Slinq(), SlinqTest.to_1, SlinqTest.to_1, (a, b) => 0).Count();
			}
		}
	}

#if !UNITY_3_5
}
#endif
