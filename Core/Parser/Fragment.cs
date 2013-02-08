// Copyright 2013 rubicon informationstechnologie gmbh
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace InjectionCop.Parser
{
  public class Fragment
  {
    private static readonly Fragment s_emptyFragment = new Fragment (FragmentType.Empty, null, false);
    private static readonly Fragment s_undefinedFragment = new Fragment (FragmentType.Empty, null, true);

    public static Fragment CreateNamed (string fragmentName)
    {
      return new Fragment (FragmentType.Named, fragmentName, false);
    }

    public static Fragment CreateLiteral ()
    {
      return new Fragment (FragmentType.Literal, null, false);
    }

    public static Fragment CreateEmpty ()
    {
      return s_emptyFragment;
    }

    public static Fragment CreateUndefined ()
    {
      return s_undefinedFragment;
    }

    private readonly FragmentType _fragmentType;
    private readonly string _fragmentName;
    private readonly bool _isUndefined;

    protected Fragment (FragmentType fragmentType, string fragmentName, bool isUndefined)
    {
      _fragmentType = fragmentType;
      _fragmentName = fragmentName;
      _isUndefined = isUndefined;
    }

    public FragmentType Type
    {
      get { return _fragmentType; }
    }

    public string FragmentName
    {
      get { return _fragmentName; }
    }

    protected bool Equals (Fragment other)
    {
      return _fragmentType == other._fragmentType && string.Equals (_fragmentName, other._fragmentName);
    }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals (null, obj))
        return false;
      if (ReferenceEquals (this, obj))
        return true;
      if (obj.GetType() != GetType())
        return false;
      return Equals ((Fragment) obj);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        return ((int) _fragmentType * 397) ^ (_fragmentName != null ? _fragmentName.GetHashCode() : 0);
      }
    }

    public static bool operator == (Fragment a, Fragment b)
    {
      return Equals (a, b);
    }

    public static bool operator != (Fragment a, Fragment b)
    {
      return !Equals (a, b);
    }

    public override string ToString ()
    {
      switch (_fragmentType)
      {
        case FragmentType.Literal:
          return "Literal";
        case FragmentType.Empty:
          return "Empty";
        default:
          return _fragmentName;
      }
    }
  }
}