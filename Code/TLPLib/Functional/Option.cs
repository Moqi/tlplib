using System;
using System.Collections.Generic;
using System.Linq;
using com.tinylabproductions.TLPLib.Extensions;

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

  public static Option<A> createOrTap<A, B>(
    this Option<A> opt, Fn<B> ifEmpty, Act<A> ifNonEmpty
  ) where B : A {
    if (opt.isEmpty) return new Some<A>(ifEmpty());

    ifNonEmpty(opt.get);
    return opt;
  }

  public static Option<A> orElse<A, B>(
    this Option<A> opt, Fn<Option<B>> other
  ) where B : A {
    return opt.isDefined ? opt : (Option<A>) other();
  }

  public static A orNull<A>(this Option<A> opt) where A : class {
    return opt.fold(() => null, _ => _);
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

public interface Option<out A> {
  bool isDefined { get; }
  bool isEmpty { get; }
  A get { get; }
  A getOrThrow(Fn<Exception> orElse);
  void each(Act<A> action);
  void voidFold(Action ifEmpty, Act<A> ifNonEmpty);
  Option<A> tap(Act<A> action);
  Option<A> filter(Fn<A, bool> predicate);
}

public class Some<A> : Option<A> {
  public Some(A value) { get = value; }

  public A getOrThrow(Fn<Exception> orElse) { return get; }

  public void each(Act<A> action) { action(get); }

  public Option<A> tap(Act<A> action) {
    action(get);
    return this;
  }

  public void voidFold(Action ifEmpty, Act<A> ifNonEmpty) {
    ifNonEmpty(get);
  }

  public Option<A> filter(Fn<A, bool> predicate) {
    return predicate(get) ? this : F.none<A>();
  }

  public bool isDefined { get { return true; } }
  public bool isEmpty { get { return false; } }
  public A get { get; private set; }

  public override string ToString() {
    return string.Format("Some({0})", get);
  }
}

public class None<A> : Option<A> {
  public static readonly None<A> instance = new None<A>();
  private None() {}

  public A getOrThrow(Fn<Exception> orElse) { throw orElse(); }

  public void each(Act<A> action) {}

  public Option<A> tap(Act<A> action) {
    return this;
  }

  public void voidFold(Action ifEmpty, Act<A> ifNonEmpty) {
    ifEmpty();
  }

  public Option<A> filter(Fn<A, bool> predicate) {
    return this;
  }

  public bool isDefined { get { return false; } }
  public bool isEmpty { get { return true; } }
  public A get { get { throw new Exception("#get on None!"); } }

  public override string ToString() {
    return "None";
  }
}
}
