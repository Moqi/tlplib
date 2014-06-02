using System;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Logger;

namespace com.tinylabproductions.TLPLib.Functional {
/** 
 * Hack to glue-on contravariant type parameter requirements
 * 
 * http://stackoverflow.com/questions/1188354/can-i-specify-a-supertype-relation-in-c-sharp-generic-constraints
 * 
 * Beware that this causes boxing for value types.
 * 
 * Also Mono compiler seems to be a lot happier with generic extension 
 * methods, than instance ones. 
 **/
public static class Option {
  static Option() {
    #if UNITY_IOS
    Log.debug(
      "iOS AOT type hints: " +
        
      new Option<byte>() + new Option<short>() + new Option<int>() + 
      new Option<long>() + new Option<float>() + new Option<double>() +
      new Option<decimal>() + new Option<bool>() + new Option<DateTime>() +
      new Option<char>()
    );
    #endif
  }

  public static IEnumerable<A> asEnum<A, B>(this Option<B> opt)
  where B : A {
    return opt.isDefined 
      ? (IEnumerable<A>) opt.get.Yield() 
      : Enumerable.Empty<A>();
  }

  public static IEnumerable<A> asEnum<A>(this Option<A> opt) {
    return opt.isDefined ? opt.get.Yield() : Enumerable.Empty<A>();
  }

  public static A getOrElse<A>(this Option<A> opt, Fn<A> orElse) {
    return opt.isDefined ? opt.get : orElse();
  }

  public static A getOrElse<A>(this Option<A> opt, A orElse) {
    return opt.isDefined ? opt.get : orElse;
  }

  public static Option<A> createOrTap<A>(
    this Option<A> opt, Fn<A> ifEmpty, Act<A> ifNonEmpty
  ) {
    if (opt.isEmpty) return new Option<A>(ifEmpty());

    ifNonEmpty(opt.get);
    return opt;
  }

  public static Option<A> orElse<A, B>(
    this Option<A> opt, Fn<Option<B>> other
  ) where B : A {
    return opt.isDefined ? opt : other().to<B, A>();
  }

  public static A orNull<A>(this Option<A> opt) where A : class {
    return opt.fold(() => null, _ => _);
  }

  public static B fold<A, B>(this Option<A> opt, Fn<B> ifEmpty, Fn<A, B> ifNonEmpty) {
    return opt.isSome ? ifNonEmpty(opt.get) : ifEmpty();
  }

  public static B fold<A, B>(this Option<A> opt, B ifEmpty, Fn<A, B> ifNonEmpty) {
    return opt.isSome ? ifNonEmpty(opt.get) : ifEmpty;
  }

  // Alias for #fold with elements switched up.
  public static B cata<A, B>(this Option<A> opt, Fn<A, B> ifNonEmpty, Fn<B> ifEmpty) {
    return opt.fold(ifEmpty, ifNonEmpty);
  }

  // Alias for #fold with elements switched up.
  public static B cata<A, B>(this Option<A> opt, Fn<A, B> ifNonEmpty, B ifEmpty) {
    return opt.fold(ifEmpty, ifNonEmpty);
  }

  public static void voidFold<A>(this Option<A> opt, Action ifEmpty, Act<A> ifNonEmpty) {
    if (opt.isSome) ifNonEmpty(opt.get);
    else ifEmpty();
  }

  // Downcast an option.
  public static Option<B> to<A, B>(this Option<A> opt) where A : B {
    return opt.map(_ => (B) _);
  }

  public static Option<B> map<A, B>(this Option<A> opt, Fn<A, B> func) {
    return opt.isDefined ? F.some(func(opt.get)) : F.none<B>();
  }

  public static Option<B> map2<A, B>(this Option<A> opt, Func<A, B> func) {
    return opt.isDefined ? F.some(func(opt.get)) : F.none<B>();
  }

  public static Option<B> flatMap<A, B>(
    this Option<A> opt, Fn<A, Option<B>> func
  ) {
    return opt.isDefined ? func(opt.get) : F.none<B>();
  }

  public static Option<Tpl<A, B>> zip<A, B>(
    this Option<A> opt1, Option<B> opt2
  ) {
    return opt1.isDefined && opt2.isDefined
      ? F.some(F.t(opt1.get, opt2.get))
      : F.none<Tpl<A, B>>();
  }
}

public struct Option<A> : IEquatable<Option<A>> {
  public static Option<A> None { get { return new Option<A>(); } }

  private readonly A value;
  public readonly bool isSome;

  public Option(A value) : this() {
    this.value = value;
    isSome = true;
  }

  public A getOrThrow(Fn<Exception> orElse) 
    { return isSome ? value : F.throws<A>(orElse()); }

  public void each(Act<A> action) { if (isSome) action(value); }

  public Option<A> tap(Act<A> action) {
    if (isSome) action(value);
    return this;
  }

  public Option<A> filter(Fn<A, bool> predicate) {
    return (isSome ? (predicate(value) ? this : F.none<A>()) : this);
  }

  public bool exists(Fn<A, bool> predicate) {
    return isSome && predicate(value);
  }

  public bool exists(A a) {
    return exists(a, Smooth.Collections.EqComparer<A>.Default);
  }

  public bool exists(A a, IEqualityComparer<A> comparer) {
    return isSome && comparer.Equals(value, a); ;
  }

  public bool isDefined { get { return isSome; } }
  public bool isEmpty { get { return ! isSome; } }

  public A get { get {
    if (isSome) return value;
    throw new IllegalStateException("#get on None!");
  } }

  public override bool Equals(object o) {
    return o is Option<A> && Equals((Option<A>)o);
  }

  public bool Equals(Option<A> other) {
    return isSome ? other.exists(value) : other.isEmpty;
  }

  public override int GetHashCode() {
    return Smooth.Collections.EqComparer<A>.Default.GetHashCode(value);
  }

  public static bool operator == (Option<A> lhs, Option<A> rhs) {
    return lhs.Equals(rhs);
  }

  public static bool operator != (Option<A> lhs, Option<A> rhs) {
    return !lhs.Equals(rhs);
  }

  public override string ToString() {
    return isSome ? "Some(" + value + ")" : "None";
  }
}
}
