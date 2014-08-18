using System;
using System.Collections;
using System.Collections.Generic;
using com.tinylabproductions.TLPLib.Concurrent;
using com.tinylabproductions.TLPLib.Extensions;
using com.tinylabproductions.TLPLib.Formats.SimpleJSON;
using com.tinylabproductions.TLPLib.Functional;
using com.tinylabproductions.TLPLib.Utilities;
using UnityEngine;

namespace com.tinylabproductions.TLPLib.Configuration {
  /**
   * Config class that fetches JSON configuration from `url`. Contents of 
   * `url` are expected to be a JSON object.
   * 
   * It takes keys in format of `key.subkey.subsubkey`. It as well tries to 
   * get platform specific overrides in following order:
   * 
   * - `key.subkey.{Platform.fullName}.subsubkey`
   * - `key.subkey.{Platform.name}.subsubkey`
   * - `key.subkey.subsubkey`
   * 
   * This does not work for fetching subconfigs, because a person that edits 
   * the configuration shouldn't know whether you're fetching values with
   * subconfig or not to correctly write the configuration.
   **/

  public class Config {
    public static Future<Config> apply(string url) {
      return ASync.StartCoroutine<JSONClass>(
        p => getConfiguration(url, p)
      ).map(data => new Config(url, "", data));
    }

    // Implementation

    public readonly string url, scope;
    public readonly JSONClass configuration;

    public Config(string url, string scope, JSONClass configuration) {
      this.url = url;
      this.scope = scope;
      this.configuration = configuration;
    }

    public override string ToString() {
      return string.Format(
        "Config(url: {0}, scope: \"{1}\", data: {2})", url, scope, configuration
      );
    }

    private static IEnumerator getConfiguration(
      string url, Promise<JSONClass> promise
    ) {
      var req = new WWW(url);
      yield return req;

      if (! string.IsNullOrEmpty(req.error)) {
        promise.completeError(new Exception(string.Format(
          "Can't load {0}: {1}", url, req.error
        )));
      }
      else {
        try {
          var json = JSON.Parse(req.text).AsObject;
          if (json == null)
            promise.completeError(new Exception(string.Format(
              "Cannot parse url '{0}' contents as JSON object:\n{1}", 
              url, req.text
            )));
          else 
            promise.completeSuccess(json);
        }
        catch (Exception e) { promise.completeError(e); }
      }
    }
  }

  public static class ConfigExts {
    private delegate Option<A> Parser<A>(JSONNode node);

    private static readonly Parser<JSONClass> jsClassParser = n => F.opt(n.AsObject);
    private static readonly Parser<string> stringParser = n => F.some(n.Value);
    private static readonly Parser<int> intParser = n => n.Value.parseInt().rightValue;
    private static readonly Parser<float> floatParser = n => n.Value.parseFloat().rightValue;
    private static readonly Parser<bool> boolParser = n => n.Value.parseBool().rightValue;

    #region getters

    public static string getString(this Config cfg, string key) 
    { return cfg.tryString(key).getOrThrow; }

    public static IList<string> getStringList(this Config cfg, string key) 
    { return cfg.tryStringList(key).getOrThrow; }

    public static int getInt(this Config cfg, string key) 
    { return cfg.tryInt(key).getOrThrow; }

    public static IList<int> getIntList(this Config cfg, string key) 
    { return cfg.tryIntList(key).getOrThrow; }

    public static float getFloat(this Config cfg, string key) 
    { return cfg.tryFloat(key).getOrThrow; }

    public static IList<float> getFloatList(this Config cfg, string key) 
    { return cfg.tryFloatList(key).getOrThrow; }

    public static bool getBool(this Config cfg, string key) 
    { return cfg.tryBool(key).getOrThrow; }

    public static IList<bool> getBoolList(this Config cfg, string key) 
    { return cfg.tryBoolList(key).getOrThrow; }

    public static Config getSubConfig(this Config cfg, string key) 
    { return cfg.trySubConfig(key).getOrThrow; }

    public static IList<Config> getSubConfigList(this Config cfg, string key) 
    { return cfg.trySubConfigList(key).getOrThrow; }

    #endregion

    #region opt getters

    public static Option<string> optString(this Config cfg, string key) {
      return cfg.get(key, stringParser).fold(_ => F.none<string>(), F.some);
    }

    public static Option<IList<string>> optStringList(this Config cfg, string key) {
      return cfg.getList(key, stringParser).
        fold(_ => F.none<IList<string>>(), F.some);
    }

    public static Option<int> optInt(this Config cfg, string key) {
      return cfg.get(key, intParser).fold(_ => F.none<int>(), F.some);
    }

    public static Option<IList<int>> optIntList(this Config cfg, string key) {
      return cfg.getList(key, intParser).
        fold(_ => F.none<IList<int>>(), F.some);
    }

    public static Option<float> optFloat(this Config cfg, string key) {
      return cfg.get(key, floatParser).fold(_ => F.none<float>(), F.some);
    }

    public static Option<IList<float>> optFloatList(this Config cfg, string key) {
      return cfg.getList(key, floatParser).
        fold(_ => F.none<IList<float>>(), F.some);
    }

    public static Option<bool> optBool(this Config cfg, string key) {
      return cfg.get(key, boolParser).fold(_ => F.none<bool>(), F.some);
    }

    public static Option<IList<bool>> optBoolList(this Config cfg, string key) {
      return cfg.getList(key, boolParser).
        fold(_ => F.none<IList<bool>>(), F.some);
    }

    public static Option<Config> optSubConfig(this Config cfg, string key) {
      return cfg.fetchSubConfig(key).fold(_ => F.none<Config>(), F.some);
    }

    public static Option<IList<Config>> optSubConfigList(this Config cfg, string key) {
      return cfg.fetchSubConfigList(key).fold(_ => F.none<IList<Config>>(), F.some);
    }

    #endregion

    #region try getters

    public static Try<string> tryString(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.get(key, stringParser).
        fold<string, string, Try<string>>(tryArgEx<string>, F.scs);
    }

    public static Try<IList<string>> tryStringList(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.getList(key, stringParser).
        fold<string, IList<string>, Try<IList<string>>>(tryArgEx<IList<string>>, F.scs);
    }

    public static Try<int> tryInt(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.get(key, n => n.Value.parseInt().rightValue).
        fold<string, int, Try<int>>(tryArgEx<int>, F.scs);
    }

    public static Try<IList<int>> tryIntList(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.getList(key, intParser).
        fold<string, IList<int>, Try<IList<int>>>(tryArgEx<IList<int>>, F.scs);
    }

    public static Try<float> tryFloat(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.get(key, n => n.Value.parseFloat().rightValue).
        fold<string, float, Try<float>>(tryArgEx<float>, F.scs);
    }

    public static Try<IList<float>> tryFloatList(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.getList(key, floatParser).
        fold<string, IList<float>, Try<IList<float>>>(tryArgEx<IList<float>>, F.scs);
    }

    public static Try<bool> tryBool(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.get(key, n => n.Value.parseBool().rightValue).
        fold<string, bool, Try<bool>>(tryArgEx<bool>, F.scs);
    }

    public static Try<IList<bool>> tryBoolList(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.getList(key, boolParser).
        fold<string, IList<bool>, Try<IList<bool>>>(tryArgEx<IList<bool>>, F.scs);
    }

    public static Try<Config> trySubConfig(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.fetchSubConfig(key).
        fold<string, Config, Try<Config>>(tryArgEx<Config>, F.scs);
    }

    public static Try<IList<Config>> trySubConfigList(this Config cfg, string key) {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      // Mono compiler bug
      return cfg.fetchSubConfigList(key).
        fold<string, IList<Config>, Try<IList<Config>>>(tryArgEx<IList<Config>>, F.scs);
    }

    private static Try<A> tryArgEx<A>(string msg) {
      return F.err<A>(new ArgumentException(msg));
    }

    #endregion

    private static Either<string, Config> fetchSubConfig(this Config cfg, string key) {
      return cfg.getConcrete(split(key), jsClassParser).
        mapRight(n => new Config(cfg.url, newScope(cfg.scope, key), n));
    }

    private static Either<string, IList<Config>> fetchSubConfigList(this Config cfg, string key) {
      return cfg.getList(key, jsClassParser).mapRight(nList => {
        var lst = F.emptyList<Config>(nList.Count);
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var idx = 0; idx < nList.Count; idx++) {
          var n = nList[idx];
          lst.Add(new Config(cfg.url, string.Format(
            "{0}[{1}]", newScope(cfg.scope, key), idx
          ), n));
        }
        return (IList<Config>) lst;
      });
    }

    private static Either<string, A> get<A>(this Config cfg, string key, Parser<A> parser) {
      var parts = split(key);

      var fullPlatform = Platform.fullName;
      var platform = Platform.name;

      // Try getting platform + subplatform, then platform, then generic key.
      var current = cfg.getConcrete(injectPlatform(parts, fullPlatform), parser);
      current = current.flatMapLeft(_ =>
        // Do not access again if fullPlatform == platform.
        fullPlatform == platform
          ? current : cfg.getConcrete(injectPlatform(parts, platform), parser)
      );
      current = current.flatMapLeft(__ => cfg.getConcrete(parts, parser));
      return current;
    }

    private static Either<string, A> getConcrete<A>(
      this Config cfg, IList<string> parts, Parser<A> parser
    ) {
      var current = cfg.configuration;

      var key = parts.mkString(".");
      foreach (var part in parts.dropRight(1)) {
        var either = fetch(current, key, part, jsClassParser);
        if (either.isLeft) return either.mapRight(_ => default(A));
        current = either.rightValue.get;
      }

      return fetch(current, key, parts[parts.Count - 1], parser);
    }

    private static Either<string, A> fetch<A>(
      JSONClass current, string key, string part, Parser<A> parser
    ) {
      if (! current.Contains(part)) return F.left<string, A>(string.Format(
        "Cannot find part '{0}' from key '{1}' in {2}",
        part, key, current
      ));
      var node = current[part];
      return parser(node).fold(
        () => F.left<string, A>(string.Format(
          "Cannot convert part '{0}' from key '{1}' to {2}. {3} Contents: {4}",
          part, key, typeof(A), node.GetType(), node
        )), F.right<string, A>
      );
    }

    private static Either<string, IList<A>> getList<A>(
      this Config cfg, string key, Parser<A> parser
    ) {
      return cfg.get(key, n => F.some(n.AsArray)).flatMapRight(arr => {
        var list = new List<A>(arr.Count);
        for (var idx = 0; idx < arr.Count; idx++) {
          var node = arr[idx];
          var parsed = parser(node);
          if (parsed.isDefined) list.Add(parsed.get);
          else return F.left<string, IList<A>>(string.Format(
            "Cannot convert '{0}'[{1}] to {2}: {3}",
            key, idx, typeof(A), node
          ));
        }
        return F.right<string, IList<A>>(list);
      });
    }

    private static string newScope(string currentScope, string key) {
      return currentScope == "" ? key : currentScope + "." + key;
    }

    private static string[] split(string key) {
      return key.Split('.');
    }

    private static string[] injectPlatform(string[] parts, string platform) {
      var output = new string[parts.Length + 1];
      Array.Copy(parts, output, parts.Length - 1);
      output[parts.Length - 1] = platform;
      output[parts.Length] = parts[parts.Length - 1];
      return output;
    }
  }
}
