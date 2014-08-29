using System;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;

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
  // Downcast an option.
  public static Option<B> to<A, B>(this Option<A> opt) where A : B {
    return opt.map<B>(_ => (B)_);
  }

  public static A orNull<A>(this Option<A> opt) where A : class {
    return opt.fold<A>(() => null, _ => _);
  }
}

public struct Option<A> {
  public static Option<A> None { get { return new Option<A>(); } }

  private readonly A value;
  public readonly bool isSome;

  public Option(A value) : this() {
    this.value = value;
    isSome = true;
  }

  public A getOrThrow(Fn<Exception> getEx) 
    { return isSome ? value : F.throws<A>(getEx()); }

  public void each(Act<A> action) { if (isSome) action(value); }

  public void onNone(Act action) { if (! isSome) action(); }

  public Option<A> tap(Act<A> action) {
    if (isSome) action(value);
    return this;
  }

  public void voidFold(Action ifEmpty, Act<A> ifNonEmpty) {
    if (isSome) ifNonEmpty(value);
    else ifEmpty();
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

  /* A quick way to get None instance for this options type. */
  public Option<A> none { get { return F.none<A>(); } }

  public IEnumerable<A> asEnum() {
    return isDefined ? get.Yield() : Enumerable.Empty<A>();
  }

  public A getOrElse(Fn<A> orElse) { return isDefined ? get : orElse(); }
  public A getOrElse(A orElse) { return isDefined ? get : orElse; }

  public Option<A> createOrTap(Fn<A> ifEmpty, Act<A> ifNonEmpty) {
    if (isEmpty) return new Option<A>(ifEmpty());

    ifNonEmpty(get);
    return this;
  }

  public Option<A> orElse(Fn<Option<A>> other) 
  { return isDefined ? this : other(); }

  public Option<A> orElse(Option<A> other) 
  { return isDefined ? this : other; }

  public B fold<B>(Fn<B> ifEmpty, Fn<A, B> ifNonEmpty) {
    return isSome ? ifNonEmpty(get) : ifEmpty();
  }

  public B fold<B>(B ifEmpty, Fn<A, B> ifNonEmpty) {
    return isSome ? ifNonEmpty(get) : ifEmpty;
  }

  // Alias for #fold with elements switched up.
  public B cata<B>(Fn<A, B> ifNonEmpty, Fn<B> ifEmpty) {
    return fold<B>(ifEmpty, ifNonEmpty);
  }

  // Alias for #fold with elements switched up.
  public B cata<B>(Fn<A, B> ifNonEmpty, B ifEmpty) {
    return fold<B>(ifEmpty, ifNonEmpty);
  }

  public Option<B> map<B>(Fn<A, B> func) {
    return isDefined ? F.some(func(get)) : F.none<B>();
  }

  public Option<B> flatMap<B>(Fn<A, Option<B>> func) {
    return isDefined ? func(get) : F.none<B>();
  }

  public Option<Tpl<A, B>> zip<B>(Option<B> opt2) {
    return isDefined && opt2.isDefined
      ? F.some(F.t(get, opt2.get)) : F.none<Tpl<A, B>>();
  }

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
