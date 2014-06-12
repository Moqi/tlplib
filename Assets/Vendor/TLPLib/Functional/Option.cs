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
  **/
public static class Option {
  public static IEnumerable<A> asEnum<A, B>(this Option<B> opt)
  where B : A {
    return opt.isDefined 
      ? (IEnumerable<A>) opt.get.Yield() 
      : Enumerable.Empty<A>();
  }

  public static IEnumerable<A> asEnum<A>(this Option<A> opt) {
    return opt.isDefined ? opt.get.Yield() : Enumerable.Empty<A>();
  }

  public static A getOrElse<A, B>(this Option<A> opt, Fn<B> orElse) 
  where B : A {
    return opt.isDefined ? opt.get : orElse();
  }

  public static A getOrElse<A, B>(this Option<A> opt, B orElse) 
  where B : A {
    return opt.isDefined ? opt.get : orElse;
  }

  public static Option<A> createOrTap<A, B>(
    this Option<A> opt, Fn<B> ifEmpty, Act<A> ifNonEmpty
  ) where B : A {
    if (opt.isEmpty) return new Option<A>(ifEmpty());

    ifNonEmpty(opt.get);
    return opt;
  }

  public static Option<A> orElse<A>(this Option<A> opt, Fn<Option<A>> other) {
    return opt.isDefined ? opt : other();
  }

  public static Option<A> orElse<A>(this Option<A> opt, Option<A> other) {
    return opt.isDefined ? opt : other;
  }

  public static A orNull<A>(this Option<A> opt) where A : class {
    return opt.fold(() => null, _ => _);
  }

  // Downcast an option.
  public static Option<B> to<A, B>(this Option<A> opt) where A : B {
    return opt.map(_ => (B) _);
  }

  public static Option<B> map<A, B>(this Option<A> opt, Fn<A, B> func) {
    return opt.isDefined ? F.some(func(opt.get)) : F.none<B>();
  }

  public static Option<B> flatMap<A, B>(
    this Option<A> opt, Fn<A, Option<B>> func
  ) {
    return opt.isDefined ? func(opt.get) : F.none<B>();
  }

  public static B fold<A, B>(
    this Option<A> opt, Fn<B> ifEmpty, Fn<A, B> ifNonEmpty
  ) {
    return opt.isDefined ? ifNonEmpty(opt.get) : ifEmpty();
  }

  public static Option<Tpl<A, B>> zip<A, B>(
    this Option<A> opt1, Option<B> opt2
  ) {
    return opt1.isDefined && opt2.isDefined
      ? F.some(F.t(opt1.get, opt2.get))
      : F.none<Tpl<A, B>>();
  }
}

public 
#if UNITY_IOS
class
#else
struct 
#endif
	Option<A> {
  private readonly A value;
  private readonly bool isSome;

#if UNITY_IOS
  public Option() {}
#endif

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

  public bool isDefined { get { return isSome; } }
  public bool isEmpty { get { return ! isSome; } }
  public A get { get { return value; } }

  public override string ToString() {
    return isSome ? "Some(" + value + ")" : "None";
  }
}
}
