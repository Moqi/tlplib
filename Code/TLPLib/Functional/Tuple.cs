/**
 * Scala code to generate this file.
 * 
 
import java.io._

val TplCs = new PrintWriter("Tpl.cs")
val fCs = new PrintWriter("F.cs")
val tName = "Tpl";
 
(2 to 22).map { i =>
  val r = 1 to i
  val genPNames = r.map(n => s"P$n")
  val types = genPNames.map("out " + _).mkString(", ")
  val sTypes = genPNames.mkString(", ")
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
    val fType = s"$iName<$sTypes>"
    TplCs.println(
s"""  [System.Serializable]
  public $kind $fType : 
  IComparable<$fType>, IEquatable<$fType> {
    $propsImpl
  
    public $iName($args)$baseConstructor { $setters; }
  
    public override string ToString() {
      return string.Format("($toStringFmt)", $toStringArgs);
    }
 
	  public override bool Equals(object o) {
      return o is $fType && this.Equals(($fType) o);
    }

    public bool Equals($fType t) {
      return (
${r.map { i => 
  s"Smooth.Collections.EqualityComparer<P$i>.Default.Equals(_$i, t._$i)"
}.mkString(" &&\n")}
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
${r.map { i => 
  s"hash = 29 * hash + Smooth.Collections.EqualityComparer<P$i>.Default.GetHashCode(_$i);"
}.mkString("\n")}
        return hash;
      }
    }

    public int CompareTo($fType other) {
      int c;
${r.map { i => 
  s"c = Smooth.Collections.Comparer<P$i>.Default.Compare(_$i, other._$i);"
}.mkString(" if (c != 0) { return c; }\n")}
      return c;
    }

    public static bool operator == ($fType lhs, $fType rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != ($fType lhs, $fType rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > ($fType lhs, $fType rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < ($fType lhs, $fType rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= ($fType lhs, $fType rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= ($fType lhs, $fType rhs) {
      return lhs.CompareTo(rhs) <= 0;
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
  
TplCs.close()
fCs.close()
 
 * 
 **/

namespace System {  [System.Serializable]
  public struct Tpl<P1, P2> : 
  IComparable<Tpl<P1, P2>>, IEquatable<Tpl<P1, P2>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; }
  
    public Tpl(P1 p1, P2 p2) : this() { _1 = p1; _2 = p2; }
  
    public override string ToString() {
      return string.Format("({0},{1})", _1, _2);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2> && this.Equals((Tpl<P1, P2>) o);
    }

    public bool Equals(Tpl<P1, P2> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2);
      return c;
    }

    public static bool operator == (Tpl<P1, P2> lhs, Tpl<P1, P2> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2> lhs, Tpl<P1, P2> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2> lhs, Tpl<P1, P2> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2> lhs, Tpl<P1, P2> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2> lhs, Tpl<P1, P2> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2> lhs, Tpl<P1, P2> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3> : 
  IComparable<Tpl<P1, P2, P3>>, IEquatable<Tpl<P1, P2, P3>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3) : this() { _1 = p1; _2 = p2; _3 = p3; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2})", _1, _2, _3);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3> && this.Equals((Tpl<P1, P2, P3>) o);
    }

    public bool Equals(Tpl<P1, P2, P3> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3> lhs, Tpl<P1, P2, P3> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3> lhs, Tpl<P1, P2, P3> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3> lhs, Tpl<P1, P2, P3> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3> lhs, Tpl<P1, P2, P3> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3> lhs, Tpl<P1, P2, P3> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3> lhs, Tpl<P1, P2, P3> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4> : 
  IComparable<Tpl<P1, P2, P3, P4>>, IEquatable<Tpl<P1, P2, P3, P4>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3})", _1, _2, _3, _4);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4> && this.Equals((Tpl<P1, P2, P3, P4>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4> lhs, Tpl<P1, P2, P3, P4> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4> lhs, Tpl<P1, P2, P3, P4> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4> lhs, Tpl<P1, P2, P3, P4> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4> lhs, Tpl<P1, P2, P3, P4> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4> lhs, Tpl<P1, P2, P3, P4> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4> lhs, Tpl<P1, P2, P3, P4> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5> : 
  IComparable<Tpl<P1, P2, P3, P4, P5>>, IEquatable<Tpl<P1, P2, P3, P4, P5>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4})", _1, _2, _3, _4, _5);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5> && this.Equals((Tpl<P1, P2, P3, P4, P5>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5> lhs, Tpl<P1, P2, P3, P4, P5> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5> lhs, Tpl<P1, P2, P3, P4, P5> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5> lhs, Tpl<P1, P2, P3, P4, P5> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5> lhs, Tpl<P1, P2, P3, P4, P5> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5> lhs, Tpl<P1, P2, P3, P4, P5> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5> lhs, Tpl<P1, P2, P3, P4, P5> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5})", _1, _2, _3, _4, _5, _6);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6> lhs, Tpl<P1, P2, P3, P4, P5, P6> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6> lhs, Tpl<P1, P2, P3, P4, P5, P6> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6> lhs, Tpl<P1, P2, P3, P4, P5, P6> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6> lhs, Tpl<P1, P2, P3, P4, P5, P6> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6> lhs, Tpl<P1, P2, P3, P4, P5, P6> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6> lhs, Tpl<P1, P2, P3, P4, P5, P6> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6})", _1, _2, _3, _4, _5, _6, _7);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7})", _1, _2, _3, _4, _5, _6, _7, _8);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8})", _1, _2, _3, _4, _5, _6, _7, _8, _9);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16) &&
Smooth.Collections.EqualityComparer<P17>.Default.Equals(_17, t._17)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P17>.Default.GetHashCode(_17);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P17>.Default.Compare(_17, other._17);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16) &&
Smooth.Collections.EqualityComparer<P17>.Default.Equals(_17, t._17) &&
Smooth.Collections.EqualityComparer<P18>.Default.Equals(_18, t._18)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P17>.Default.GetHashCode(_17);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P18>.Default.GetHashCode(_18);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P17>.Default.Compare(_17, other._17); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P18>.Default.Compare(_18, other._18);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16) &&
Smooth.Collections.EqualityComparer<P17>.Default.Equals(_17, t._17) &&
Smooth.Collections.EqualityComparer<P18>.Default.Equals(_18, t._18) &&
Smooth.Collections.EqualityComparer<P19>.Default.Equals(_19, t._19)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P17>.Default.GetHashCode(_17);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P18>.Default.GetHashCode(_18);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P19>.Default.GetHashCode(_19);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P17>.Default.Compare(_17, other._17); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P18>.Default.Compare(_18, other._18); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P19>.Default.Compare(_19, other._19);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16) &&
Smooth.Collections.EqualityComparer<P17>.Default.Equals(_17, t._17) &&
Smooth.Collections.EqualityComparer<P18>.Default.Equals(_18, t._18) &&
Smooth.Collections.EqualityComparer<P19>.Default.Equals(_19, t._19) &&
Smooth.Collections.EqualityComparer<P20>.Default.Equals(_20, t._20)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P17>.Default.GetHashCode(_17);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P18>.Default.GetHashCode(_18);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P19>.Default.GetHashCode(_19);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P20>.Default.GetHashCode(_20);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P17>.Default.Compare(_17, other._17); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P18>.Default.Compare(_18, other._18); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P19>.Default.Compare(_19, other._19); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P20>.Default.Compare(_20, other._20);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; } public P21 _21 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; _21 = p21; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16) &&
Smooth.Collections.EqualityComparer<P17>.Default.Equals(_17, t._17) &&
Smooth.Collections.EqualityComparer<P18>.Default.Equals(_18, t._18) &&
Smooth.Collections.EqualityComparer<P19>.Default.Equals(_19, t._19) &&
Smooth.Collections.EqualityComparer<P20>.Default.Equals(_20, t._20) &&
Smooth.Collections.EqualityComparer<P21>.Default.Equals(_21, t._21)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P17>.Default.GetHashCode(_17);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P18>.Default.GetHashCode(_18);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P19>.Default.GetHashCode(_19);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P20>.Default.GetHashCode(_20);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P21>.Default.GetHashCode(_21);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P17>.Default.Compare(_17, other._17); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P18>.Default.Compare(_18, other._18); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P19>.Default.Compare(_19, other._19); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P20>.Default.Compare(_20, other._20); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P21>.Default.Compare(_21, other._21);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 

  }
  [System.Serializable]
  public struct Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> : 
  IComparable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>>, IEquatable<Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>> {
    public P1 _1 { get; private set; } public P2 _2 { get; private set; } public P3 _3 { get; private set; } public P4 _4 { get; private set; } public P5 _5 { get; private set; } public P6 _6 { get; private set; } public P7 _7 { get; private set; } public P8 _8 { get; private set; } public P9 _9 { get; private set; } public P10 _10 { get; private set; } public P11 _11 { get; private set; } public P12 _12 { get; private set; } public P13 _13 { get; private set; } public P14 _14 { get; private set; } public P15 _15 { get; private set; } public P16 _16 { get; private set; } public P17 _17 { get; private set; } public P18 _18 { get; private set; } public P19 _19 { get; private set; } public P20 _20 { get; private set; } public P21 _21 { get; private set; } public P22 _22 { get; private set; }
  
    public Tpl(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, P12 p12, P13 p13, P14 p14, P15 p15, P16 p16, P17 p17, P18 p18, P19 p19, P20 p20, P21 p21, P22 p22) : this() { _1 = p1; _2 = p2; _3 = p3; _4 = p4; _5 = p5; _6 = p6; _7 = p7; _8 = p8; _9 = p9; _10 = p10; _11 = p11; _12 = p12; _13 = p13; _14 = p14; _15 = p15; _16 = p16; _17 = p17; _18 = p18; _19 = p19; _20 = p20; _21 = p21; _22 = p22; }
  
    public override string ToString() {
      return string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21})", _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22);
    }
 
  public override bool Equals(object o) {
      return o is Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> && this.Equals((Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22>) o);
    }

    public bool Equals(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> t) {
      return (
Smooth.Collections.EqualityComparer<P1>.Default.Equals(_1, t._1) &&
Smooth.Collections.EqualityComparer<P2>.Default.Equals(_2, t._2) &&
Smooth.Collections.EqualityComparer<P3>.Default.Equals(_3, t._3) &&
Smooth.Collections.EqualityComparer<P4>.Default.Equals(_4, t._4) &&
Smooth.Collections.EqualityComparer<P5>.Default.Equals(_5, t._5) &&
Smooth.Collections.EqualityComparer<P6>.Default.Equals(_6, t._6) &&
Smooth.Collections.EqualityComparer<P7>.Default.Equals(_7, t._7) &&
Smooth.Collections.EqualityComparer<P8>.Default.Equals(_8, t._8) &&
Smooth.Collections.EqualityComparer<P9>.Default.Equals(_9, t._9) &&
Smooth.Collections.EqualityComparer<P10>.Default.Equals(_10, t._10) &&
Smooth.Collections.EqualityComparer<P11>.Default.Equals(_11, t._11) &&
Smooth.Collections.EqualityComparer<P12>.Default.Equals(_12, t._12) &&
Smooth.Collections.EqualityComparer<P13>.Default.Equals(_13, t._13) &&
Smooth.Collections.EqualityComparer<P14>.Default.Equals(_14, t._14) &&
Smooth.Collections.EqualityComparer<P15>.Default.Equals(_15, t._15) &&
Smooth.Collections.EqualityComparer<P16>.Default.Equals(_16, t._16) &&
Smooth.Collections.EqualityComparer<P17>.Default.Equals(_17, t._17) &&
Smooth.Collections.EqualityComparer<P18>.Default.Equals(_18, t._18) &&
Smooth.Collections.EqualityComparer<P19>.Default.Equals(_19, t._19) &&
Smooth.Collections.EqualityComparer<P20>.Default.Equals(_20, t._20) &&
Smooth.Collections.EqualityComparer<P21>.Default.Equals(_21, t._21) &&
Smooth.Collections.EqualityComparer<P22>.Default.Equals(_22, t._22)
      );
    }

    public override int GetHashCode() {
      unchecked {
        int hash = 17;
hash = 29 * hash + Smooth.Collections.EqualityComparer<P1>.Default.GetHashCode(_1);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P2>.Default.GetHashCode(_2);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P3>.Default.GetHashCode(_3);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P4>.Default.GetHashCode(_4);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P5>.Default.GetHashCode(_5);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P6>.Default.GetHashCode(_6);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P7>.Default.GetHashCode(_7);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P8>.Default.GetHashCode(_8);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P9>.Default.GetHashCode(_9);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P10>.Default.GetHashCode(_10);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P11>.Default.GetHashCode(_11);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P12>.Default.GetHashCode(_12);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P13>.Default.GetHashCode(_13);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P14>.Default.GetHashCode(_14);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P15>.Default.GetHashCode(_15);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P16>.Default.GetHashCode(_16);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P17>.Default.GetHashCode(_17);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P18>.Default.GetHashCode(_18);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P19>.Default.GetHashCode(_19);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P20>.Default.GetHashCode(_20);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P21>.Default.GetHashCode(_21);
hash = 29 * hash + Smooth.Collections.EqualityComparer<P22>.Default.GetHashCode(_22);
        return hash;
      }
    }

    public int CompareTo(Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> other) {
      int c;
c = Smooth.Collections.Comparer<P1>.Default.Compare(_1, other._1); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P2>.Default.Compare(_2, other._2); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P3>.Default.Compare(_3, other._3); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P4>.Default.Compare(_4, other._4); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P5>.Default.Compare(_5, other._5); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P6>.Default.Compare(_6, other._6); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P7>.Default.Compare(_7, other._7); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P8>.Default.Compare(_8, other._8); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P9>.Default.Compare(_9, other._9); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P10>.Default.Compare(_10, other._10); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P11>.Default.Compare(_11, other._11); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P12>.Default.Compare(_12, other._12); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P13>.Default.Compare(_13, other._13); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P14>.Default.Compare(_14, other._14); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P15>.Default.Compare(_15, other._15); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P16>.Default.Compare(_16, other._16); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P17>.Default.Compare(_17, other._17); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P18>.Default.Compare(_18, other._18); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P19>.Default.Compare(_19, other._19); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P20>.Default.Compare(_20, other._20); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P21>.Default.Compare(_21, other._21); if (c != 0) { return c; }
c = Smooth.Collections.Comparer<P22>.Default.Compare(_22, other._22);
      return c;
    }

    public static bool operator == (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator != (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> rhs) {
      return !lhs.Equals(rhs);
    }

    public static bool operator > (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> rhs) {
      return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator < (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> rhs) {
      return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> rhs) {
      return lhs.CompareTo(rhs) >= 0;
    }

    public static bool operator <= (Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> lhs, Tpl<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22> rhs) {
      return lhs.CompareTo(rhs) <= 0;
    } 
  }

}
