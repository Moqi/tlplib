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
}

public interface Option<out A> {
  bool isDefined { get; }
  bool isEmpty { get; }
  A get { get; }
  A getOrThrow(Fn<Exception> orElse);
  void each(Act<A> action);
  Option<A> tap(Act<A> action);
  Option<B> map<B>(Fn<A, B> func);
  Option<To> flatMap<To>(Fn<A, Option<To>> func);
  B fold<B>(Fn<B> ifEmpty, Fn<A, B> ifNonEmpty);
  void voidFold(Action ifEmpty, Act<A> ifNonEmpty);
  Option<A> filter(Fn<A, bool> predicate);
  Option<Tpl<A, B>> zip<B>(Option<B> opt);
}

public struct Some<A> : Option<A> {
  public Some(A value) : this() { get = value; }

  public A getOrThrow(Fn<Exception> orElse) { return get; }

  public void each(Act<A> action) { action(get); }

  public Option<A> tap(Act<A> action) {
    action(get);
    return this;
  }

  public Option<B> map<B>(Fn<A, B> func) {
    return new Some<B>(func(get));
  }

  public Option<B> flatMap<B>(Fn<A, Option<B>> func) { return func(get); }

  public B fold<B>(Fn<B> ifEmpty, Fn<A, B> ifNonEmpty) {
    return ifNonEmpty(get);
  }

  public void voidFold(Action ifEmpty, Act<A> ifNonEmpty) {
    ifNonEmpty(get);
  }

  public Option<A> filter(Fn<A, bool> predicate) {
    return predicate(get) ? this : F.none<A>();
  }

  public Option<Tpl<A, B>> zip<B>(Option<B> opt) {
    return opt.isDefined ? F.some(F.t(get, opt.get)) : F.none<Tpl<A, B>>();
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

  public Option<B> map<B>(Fn<A, B> func) {
    return F.none<B>();
  }

  public Option<B> flatMap<B>(Fn<A, Option<B>> func) {
    return F.none<B>();
  }

  public B fold<B>(Fn<B> ifEmpty, Fn<A, B> ifNonEmpty) {
    return ifEmpty();
  }

  public void voidFold(Action ifEmpty, Act<A> ifNonEmpty) {
    ifEmpty();
  }

  public Option<A> filter(Fn<A, bool> predicate) {
    return this;
  }

  public Option<Tpl<A, B>> zip<B>(Option<B> opt) {
    return F.none<Tpl<A, B>>();
  }

  public bool isDefined { get { return false; } }
  public bool isEmpty { get { return true; } }
  public A get { get { throw new Exception("#get on None!"); } }

  public override string ToString() {
    return "None";
  }
}
}
