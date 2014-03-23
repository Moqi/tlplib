using System;

namespace com.tinylabproductions.TLPLib.Functional.Matching {
  public interface IVoidMatcher<in Base> where Base : class {
    IVoidMatcher<Base> when<T>(Act<T> onMatch)
    where T : class;

    IVoidMatcher<Base> whenSealed<T>(Act<T> onMatch)
    where T : class, Base;
  }

  public interface IMatcher<in Base, Return> where Base : class {
    IMatcher<Base,Return> when<T>(Fn<T, Return> onMatch)
    where T : class;

    IMatcher<Base,Return> whenSealed<T>(Fn<T, Return> onMatch)
    where T : class, Base;

    Return get();
    Return getOrElse(Fn<Return> elseFunc);
  }

  public class MatchError : Exception {
    public MatchError(string message) : base(message) { }
  }

  public class Matcher<Base, Return> 
  : IVoidMatcher<Base>, IMatcher<Base, Return>
  where Base : class {
    private readonly Base subject;

    public Matcher(Base subject) {
      this.subject = subject;
    }

    public IVoidMatcher<Base> when<T>(Act<T> onMatch) 
    where T : class {
      var casted = subject as T;
      if (casted == null) return this;

      onMatch(casted);
      return new SuccessfulMatcher<Base, Unit>(F.unit());
    }

    public IVoidMatcher<Base> whenSealed<T>(Act<T> onMatch) 
    where T : class, Base { return when(onMatch); }

    public IMatcher<Base, Return> when<T>(Fn<T, Return> onMatch)
    where T : class {
      var casted = subject as T;
      if (casted != null)
        return new SuccessfulMatcher<Base, Return>(onMatch.Invoke(casted));

      return this;
    }

    public IMatcher<Base, Return> whenSealed<T>(Fn<T, Return> onMatch)
    where T : class, Base { return when(onMatch); }

    public Return get() {
      throw new MatchError(string.Format(
        "Subject {0} of type {1} couldn't be matched!", subject, typeof(Base)
      ));
    }

    public Return getOrElse(Fn<Return> elseFunc) { return elseFunc.Invoke(); }
  }

  public class SuccessfulMatcher<Base, Return> 
  : IVoidMatcher<Base>, IMatcher<Base, Return>
  where Base : class {
    private readonly Return result;

    public SuccessfulMatcher(Return result) {
      this.result = result;
    }

    public IVoidMatcher<Base> when<T>(Act<T> onMatch) 
    where T : class { return this; }

    public IVoidMatcher<Base> whenSealed<T>(Act<T> onMatch)
    where T : class, Base { return this; }

    public IMatcher<Base, Return> when<T>(Fn<T, Return> onMatch)
    where T : class { return this; }

    public IMatcher<Base, Return> whenSealed<T>(Fn<T, Return> onMatch)
    where T : class, Base { return this; }

    public Return get() { return result; }

    public Return getOrElse(Fn<Return> elseFunc) { return get(); }
  }

  public class MatcherBuilder<T> where T : class {
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
