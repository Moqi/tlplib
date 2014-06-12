Tiny Lab Productions Library
============================

This is library which we, [Tiny Lab Productions](www.tinylabproductions.com) use
in our Unity3D games. 

It contains various things, including but not limited to:

* Android utilities.
* BiMap.
* Promises & Futures, CoRoutine helpers.
* Various data structures.
* Iter library - allocation free enumerators.
* A bunch of extension methods.
* JSON parser/emmiter.
* Functional utilities: Option, Lazy, Tuple, Either, Try, Unit, co-variant functions and actions, rudimentary pattern matching.
* Reactive extensions: observables, reactive references, lists and list views.
* Tween utilities: mainly to make tweens type-safe.
* Various other misc utilities.

Requirements
------------

This library requires at least Unity 4.5.

Disclaimer
----------

You are free to use this for your own games. Patches and improvements are welcome. A mention in your game credits would be nice.

If you are considering using this it would be also nice if you contacted me (Artūras Šlajus, arturas@tinylabproductions.com) so we could create a community around this.

Known bugs
----------

Covariant interfaces
~~~~~~~~~~~~~~~~~~~~

There's a bug in current version of Mono Runtime which Unity 4.3.3f uses. 
This is not something we can fix and we have reported this bug to Unity.

If you have an interface and a class implementing it you'll want to 
explicitly specify the interface type for operations working on covariant
containers.

For example, using Future:

    // TLPAds is an interface.
    public readonly Future<TLPAds> ads;

    ...
    
    // configuration is Future<TLPConfig>, which is a class.
    // Mono runtime covariance bug. Crashes otherwise.
    ads = configuration.map<TLPAds>(c => new TlpAdsImpl(c));

Or using Option:

    // Again, IAdNetworkProvider is an interface
    public static Option<IAdNetworkProvider> apply(TLPConfig config) {

    ...

    // Mono runtime covariance bug. Crashes otherwise.
    return F.some<IAdNetworkProvider>(new AdMobAdNet(
      ...
    ));

Report this crash to Unity when you encounter it - the more people report it,
the better. Perhaps they'll even fix it! ;)

AOT bugs
~~~~~~~~

AOT is used in iOS builds.

Generic instance methods does not work at all. E.g.:

    public T doStuff<T>(T param) { ... }

will just get ignored by AOT compiler. Strangely if we move the same method
to a static class and make it an extension method, it works just fine.

More info can be found at http://www.reentrygames.com/unity-3d-using-extension-methods-to-solve-problems-with-generics-and-aot-on-ios/

AOT compiler is pretty stupid with generic value types as well.

For example:

    var either = F.left<int, float>(3);
    either.mapLeft(_ => _ * 3);

This will fail at runtime, because #mapLeft uses Option<float> under the hood
and AOT compiler doesn't pick it up in AOT compilation time.

Current workaround is to use compiler hints (stupid and direct code which monoc
can pick up, so it generates the appropriate machine code) for every single
value type that you can imagine and then never define value types yourself.

Obviously this needs a better solution, but it is currently out of scope for us.

where B : A constraint
~~~~~~~~~~~~~~~~~~~~~~

Following pattern causes boxing for structs and causes problems in web player:

  public static B foo(this A a, B b) where B : A {
    return b;
  }

General iOS
~~~~~~~~~~~

Whether this library works on iOS is questionable. We've succesfully built and
ran our own games with it, but there might be hidden bugs out there. If 
something breaks, expect to fix it yourself (and then do a pull request). But it
shouldn't. Hopefully.

Using in your project
---------------------

There are various ways this library can be used in your project, but I suggest 
using a git subtree and symlinking the required code into your assets folder.

In general it goes like this:

* cd your_project_dir

Add remote repository reference:

* git remote add -f tlplib git@github.com:tinylabproductions/tlplib.git

Merge the subtree:

* git subtree add -P vendor/tlplib --squash tlplib master

Link the code into your assets:

(from git bash)

* vendor/tlplib/setup/setup.sh

And you should be good to go.

Don't forget that when you clone your git repository to a new place, your 
```tlplib``` remote will be lost so you will need to readd it.

Alternatively you can just copy the whole Assets folder to your project and be done with it.

### Defining compiler constants ###

There are bits of code that depend on third party libraries that you might not
want to use in your project. They are disabled by default via precompiler 
defines.

If you wish to use them, you need to define the constants in Unity3D (Menu Bar > Edit > Project Settings > Player > (your platform) > Other Settings > Scripting Define Symbols).

* GOTWEEN - if you are using [GoKit](https://github.com/prime31/GoKit).
* DFGUI - if you are using [Daikon Forge GUI](http://www.daikonforge.com/dfgui/). Beware that this also uses GOTWEEN as well.
* UNITY_TEST - to enable unit tests.

Getting latest updates
----------------------

Is as simple as:

* git subtree pull -P vendor/tlplib tlplib master

Submitting your changes
-----------------------

Fork this repository on github and add it as a remote, e.g.:

* git remote add tlplib_my git@github.com:thisisme/tlplib.git

Commit your updates to your repository and push them to forked repo:

* git subtree push -P vendor/tlplib tlplib_my

And finally send me a pull request :)

Questions and feedback
----------------------

Contact me at <arturas@tinylabproductions.com>.
