using System;

namespace com.tinylabproductions.TLPLib.Functional.Matching {
  public interface IVoidMatcher<Base> where Base : class {
    IVoidMatcher<Base> when<T>(Act<T> onMatch) where T : Base;
    void orElse(Act<Base> act);
  }

  public interface IMatcher<in Base, Return> where Base : class {
    IMatcher<Base,Return> when<T>(Fn<T, Return> onMatch) where T : class, Base;
    /* Unrestrained version of when, needed to work around C# type system 
     * sometimes. */
    IMatcher<Base, Return> whenU<T>(Fn<T, Return> onMatch) where T : class;

    Return get();
    Return getOrElse(Fn<Return> elseFunc);
    Return getOrElse(Return elseVal);
  }

  public class MatchError : Exception {
    public MatchError(string message) : base(message) { }
  }

  public struct Matcher<Base, Return> : IVoidMatcher<Base>, IMatcher<Base, Return>
  where Base : class {
    private readonly Base subject;

    public Matcher(Base subject) {
      this.subject = subject;
    }

    public IVoidMatcher<Base> when<T>(Act<T> onMatch) where T : Base {
      if (subject is T) {
        onMatch((T) subject);
        return new SuccessfulMatcher<Base, Unit>(F.unit);
      }
      else return this;
    }

    public void orElse(Act<Base> act) { act(subject); }

    public IMatcher<Base, Return> when<T>(Fn<T, Return> onMatch)
    where T : class, Base { return whenU(onMatch); }

    public IMatcher<Base, Return> whenU<T>(Fn<T, Return> onMatch) where T : class {
      var casted = subject as T;
      if (casted != null) {
        return new SuccessfulMatcher<Base, Return>(onMatch.Invoke(casted));
      }

      return this;
    }

    public Return get() {
      throw new MatchError(string.Format(
        "Subject {0} of type {1} couldn't be matched!", subject, typeof(Base)
      ));
    }

    public Return getOrElse(Fn<Return> elseFunc) { return elseFunc.Invoke(); }
    public Return getOrElse(Return elseVal) { return elseVal; }
  }

  public struct SuccessfulMatcher<Base, Return> 
  : IVoidMatcher<Base>, IMatcher<Base, Return>
  where Base : class {
    private readonly Return result;

    public SuccessfulMatcher(Return result) {
      this.result = result;
    }

    public IVoidMatcher<Base> when<T>(Act<T> onMatch) 
    where T : Base { return this; }

    public void orElse(Act<Base> act) {}

    public IMatcher<Base, Return> when<T>(Fn<T, Return> onMatch)
    where T : class, Base { return this; }

    public IMatcher<Base, Return> whenU<T>(Fn<T, Return> onMatch) 
    where T : class { return this; }

    public Return get() { return result; }

    public Return getOrElse(Fn<Return> elseFunc) { return get(); }
    public Return getOrElse(Return elseVal) { return get(); }
  }

  public struct MatcherBuilder<T> where T : class {
    private readonly T subject;

    public MatcherBuilder(T subject) {
      this.subject = subject;
    }

    public IMatcher<T, Return> returning<Return>() {
      return new Matcher<T, Return>(subject);
    }
  }

  public static class Match {
    public static MatcherBuilder<T> match<T>(this T subject)
    where T : class { return new MatcherBuilder<T>(subject); }

    public static IVoidMatcher<T> matchVoid<T>(this T subject)
    where T : class { return new Matcher<T, Unit>(subject); }
  }
}
