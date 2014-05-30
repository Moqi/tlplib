/**
 *
 * 
 
import java.io._

val tupleCs = new PrintWriter("Tuple.cs")
val fCs = new PrintWriter("F.cs")
val tName = "Tpl";
 
(2 to 22).map { i =>
  val r = 1 to i
  val types = r.map(n => s"out P$n").mkString(", ")
  val sTypes = r.map(n => s"P$n").mkString(", ")
  val args = r.map(n => s"P$n p$n").mkString(", ")
  val argsNoTypes = r.map(n => s"p$n").mkString(", ")
  val setters = r.map(n => s"_$n = p$n").mkString("; ")
  val props = r.map(n => s"P$n _$n { get; }").mkString(" ")
  val propsImpl = r.map(n => s"public P$n _$n { get; private set; }").mkString(" ")
  val toStringFmt = r.map(n => s"{${n - 1}}").mkString(",")
  val toStringArgs = r.map(n => s"_$n").mkString(", ")
  val iNames = Map(
    "struct" -> ("", " : this()")
  ).map { case (kind, (prefix, baseConstructor)) =>
    val iName = s"${prefix}${tName}"
    tupleCs.println(
s"""  public $kind $iName<$sTypes> {
    $propsImpl
  
    public $iName($args)$baseConstructor { $setters; }
  
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
  public struct Tpl<P1, P2> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; }
  
    public Tpl(P1 p1, P2 p2) : this() { _1 = p1; _2 = p2; }
  
    public override string ToString() {
      return string.Format("({0},{1})", _1, _2);
    } 
  }
  public struct Tpl<P1, P2, P3> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3) : this() { _1 = p1; _2 = p2; _3 = p3; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2})", _1, _2, _3);
    } 
  }
  public struct Tpl<P1, P2, P3, P4> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3})", _1, _2, _3, _4);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4})", _1, _2, _3, _4, _5);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5})", _1, _2, _3, _4, _5, _6);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6})", _1, _2, _3, _4, _5, _6, _7);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7})", _1, _2, _3, _4, _5, _6, _7, _8);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8})", _1, _2, _3, _4, _5, _6, _7, _8, _9);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; } public P21 _21 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; _21 = p21; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21);
    } 
  }
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; } public P21 _21 { get; private set; } public P22 _22 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21, P22 p22) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; _21 = p21; _22 = p22; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22);
    } 
  }

}
