﻿using System;
using System.Collections.Generic;

namespace com.tinylabproductions.TLPLib.Functional {
  public static class F {
    public static Option<A> opt<A>(A value) where A : class {
      return value == null 
        ? (Option<A>) None<A>.instance 
        : new Some<A>(value);
    }

    public static Option<A> some<A>(A value) { return new Some<A>(value); }
    public static Option<A> none<A>() { return None<A>.instance; }

    public static Either<A, B> left<A, B>(A value) { return new Left<A, B>(value); }
    public static Either<A, B> right<A, B>(B value) { return new Right<A, B>(value); }

    // Exception thrower which "returns" a value for use in expressions.
    public static A throws<A>(Exception ex) { throw ex; }

    public static Try<A> doTry<A>(Fn<A> f) {
      try { return scs(f()); }
      catch (Exception e) { return err<A>(e); }
    }
    public static Try<Unit> doTry(Act action) {
      return doTry(() => { action(); return unit; });
    }
    public static Try<A> scs<A>(A value) { return new Success<A>(value); }
    public static Try<A> err<A>(Exception ex) { return new Error<A>(ex); }

    public static List<A> list<A>(params A[] args) {
      return new List<A>(args);
    }

    public static IList<A> ilist<A>(params A[] args) { return list(args); }

    public static Unit unit { get { return Unit.instance; } }

    public static Lazy<A> lazy<A>(Fn<A> func) {
      return new LazyImpl<A>(func);
    }

    public static Act andThen(this Act first, Act second) {
      return () => {
        first();
        second();
      };
    }

#region Generated code
    public static Tpl<P1, P2> t<P1, P2>(P1 p1, P2 p2) { return new TplImpl<P1, P2>(p1, p2); }
    public static Tpl<P1, P2> ht<P1, P2>(P1 p1, P2 p2) { return new HTplImpl<P1, P2>(p1, p2); }
    public static Tpl<P1, P2, P3> t<P1, P2, P3>(P1 p1, P2 p2, P3 p3) { return new TplImpl<P1, P2, P3>(p1, p2, p3); }
    public static Tpl<P1, P2, P3> ht<P1, P2, P3>(P1 p1, P2 p2, P3 p3) { return new HTplImpl<P1, P2, P3>(p1, p2, p3); }
    public static Tpl<P1, P2, P3, P4> t<P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) { return new TplImpl<P1, P2, P3, P4>(p1, p2, p3, p4); }
    public static Tpl<P1, P2, P3, P4> ht<P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) { return new HTplImpl<P1, P2, P3, P4>(p1, p2, p3, p4); }
    public static Tpl<P1, P2, P3, P4, P5> t<P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) { return new TplImpl<P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5); }
    public static Tpl<P1, P2, P3, P4, P5> ht<P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) { return new HTplImpl<P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5); }
    public static Tpl<P1, P2, P3, P4, P5, P6> t<P1, P2, P3, P4, P5, P6>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6) { return new TplImpl<P1, P2, P3, P4, P5, P6>(p1, p2, p3, p4, p5, p6); }
    public static Tpl<P1, P2, P3, P4, P5, P6> ht<P1, P2, P3, P4, P5, P6>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6) { return new HTplImpl<P1, P2, P3, P4, P5, P6>(p1, p2, p3, p4, p5, p6); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7> t<P1, P2, P3, P4, P5, P6, P7>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7>(p1, p2, p3, p4, p5, p6, p7); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7> ht<P1, P2, P3, P4, P5, P6, P7>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7>(p1, p2, p3, p4, p5, p6, p7); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8> t<P1, P2, P3, P4, P5, P6, P7, P8>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8>(p1, p2, p3, p4, p5, p6, p7, p8); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8> ht<P1, P2, P3, P4, P5, P6, P7, P8>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8>(p1, p2, p3, p4, p5, p6, p7, p8); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> t<P1, P2, P3, P4, P5, P6, P7, P8, P9>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9>(p1, p2, p3, p4, p5, p6, p7, p8, p9); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9>(p1, p2, p3, p4, p5, p6, p7, p8, p9); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> t<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21, P22 p22) { return new TplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22); }
    public static Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> ht<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21, P22 p22) { return new HTplImpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22); }
#endregion

  }
}
