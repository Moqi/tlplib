using System;
using UnityEngine;

namespace com.tinylabproductions.TLPLib {
  /* Raised when WWW.error was not empty. */
  public class WWWException : Exception {
    public readonly WWW www;

    public WWWException(WWW www) : base(www.error) 
    { this.www = www; }
  }
}
