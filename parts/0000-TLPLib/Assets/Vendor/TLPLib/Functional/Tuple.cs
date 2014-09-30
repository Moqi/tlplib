/**
 * Scala code to generate this file.
 * 
 
import java.io._

val tupleCs = new PrintWriter("Tuple.cs")
val fCs = new PrintWriter("F.cs")
val tName = "Tpl";
 
(2 to 22).map { i =>
  val r = 1 to i
  val genPNames = r.map(n => s"P$n")
  val sTypes = genPNames.mkString(", ")
  val args = r.map(n => s"P$n p$n").mkString(", ")
  val argsNoTypes = r.map(n => s"p$n").mkString(", ")
  val propsNoTypes = r.map(n => s"_$n").mkString(", ")
  val setters = r.map(n => s"_$n = p$n").mkString("; ")
  val propsImpl = r.map(n => s"public P$n _$n { get; private set; }").mkString(" ")
  val toStringFmt = r.map(n => s"{${n - 1}}").mkString(",")
  val toStringArgs = r.map(n => s"_$n").mkString(", ")
  val iNames = Map(
    "\n#if UNITY_IOS\nclass\n#else\nstruct\n#endif\n" -> ("", "\n#if ! UNITY_IOS\n : this()\n#endif\n")
  ).map { case (kind, (prefix, baseConstructor)) =>
    val iName = s"${prefix}${tName}"
    val fType = s"$iName<$sTypes>"
    tupleCs.println(
s"""  [System.Serializable]
  public $kind $fType {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    $propsImpl
  
    public $iName($args)$baseConstructor { $setters; }
  
    // Unapply.
    public void ua(Act<$sTypes> f) { f($propsNoTypes); }
 
    // Unapply with function.
    public R ua<R>(Fn<$sTypes, R> f) { return f($propsNoTypes); } 
 
    public override string ToString() {
      return string.Format("($toStringFmt)", $toStringArgs);
    }

  }""")
    s"${prefix.toLowerCase}t" -> iName
  }
  (i, iNames, sTypes, args, argsNoTypes)
}.foreach { case (i, iNames, sTypes, args, argsNoTypes) =>
  iNames.foreach { case (prefix, iName) =>
    fCs.println(
s"public static $tName<$sTypes> $prefix<$sTypes>($args) { return new $iName<$sTypes>($argsNoTypes); }"
    )
  }
}
  
tupleCs.close()
fCs.close()
 
 * 
 **/

namespace System {
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; }

    public Tpl(P1 p1, P2 p2)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; }

    // Unapply.
    public void ua(Act<P1, P2> f) { f(_1, _2); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, R> f) { return f(_1, _2); }

    public override string ToString() {
      return string.Format("({0},{1})", _1, _2);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; }

    // Unapply.
    public void ua(Act<P1, P2, P3> f) { f(_1, _2, _3); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, R> f) { return f(_1, _2, _3); }

    public override string ToString() {
      return string.Format("({0},{1},{2})", _1, _2, _3);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4> f) { f(_1, _2, _3, _4); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, R> f) { return f(_1, _2, _3, _4); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3})", _1, _2, _3, _4);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5> f) { f(_1, _2, _3, _4, _5); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, R> f) { return f(_1, _2, _3, _4, _5); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4})", _1, _2, _3, _4, _5);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6> f) { f(_1, _2, _3, _4, _5, _6); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, R> f) { return f(_1, _2, _3, _4, _5, _6); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5})", _1, _2, _3, _4, _5, _6);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7> f) { f(_1, _2, _3, _4, _5, _6, _7); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, R> f) { return f(_1, _2, _3, _4, _5, _6, _7); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6})", _1, _2, _3, _4, _5, _6, _7);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8> f) { f(_1, _2, _3, _4, _5, _6, _7, _8); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7})", _1, _2, _3, _4, _5, _6, _7, _8);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8})", _1, _2, _3, _4, _5, _6, _7, _8, _9);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; } public P21 _21 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; _21 = p21; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21);
    }

  }
  [System.Serializable]
  public
#if UNITY_IOS
class
#else
 struct
#endif
 Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> {
    // Unity compiler bug. If you make these readonly values there's a bug 
    // where accessing _* elements yields wrong results:
    // https://gist.github.com/arturaz/1645200a73ac55eaf72e
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; } public P21 _21 { get; private set; } public P22 _22 { get; private set; }

    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21, P22 p22)
#if ! UNITY_IOS
      : this()
#endif
 { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; _21 = p21; _22 = p22; }

    // Unapply.
    public void ua(Act<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> f) { f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22); }

    // Unapply with function.
    public R ua<R>(Fn<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, R> f) { return f(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22); }

    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22);
    }

  }

}
