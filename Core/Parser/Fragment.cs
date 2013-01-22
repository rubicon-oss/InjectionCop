using System;

namespace InjectionCop.Parser
{
  public class Fragment
  {
    public static Fragment CreateNamed (string fragmentName)
    {
      return new Fragment (Type.NamedFragment, fragmentName);
    }

    public static Fragment CreateLiteral ()
    {
      return new Fragment (Type.Literal, null);
    }

    public enum Type
    {
      Literal = 0,
      NamedFragment = 1
    }

    private readonly Type _type;
    private readonly string _fragmentName;

    private Fragment (Type type, string fragmentName)
    {
      _type = type;
      _fragmentName = fragmentName;
    }

    public Type FragmentType
    {
      get { return _type; }
    }

    public string FragmentName
    {
      get { return _fragmentName; }
    }

    protected bool Equals (Fragment other)
    {
      return _type == other._type && string.Equals (_fragmentName, other._fragmentName);
    }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals (null, obj))
        return false;
      if (ReferenceEquals (this, obj))
        return true;
      if (obj.GetType() != this.GetType())
        return false;
      return Equals ((Fragment) obj);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        return ((int) _type * 397) ^ (_fragmentName != null ? _fragmentName.GetHashCode() : 0);
      }
    }

    public static bool operator == (Fragment a, Fragment b)
    {
      return object.Equals (a, b);
    }
    
    public static bool operator != (Fragment a, Fragment b)
    {
      return !object.Equals (a, b);
    }

    public override string ToString ()
    {
      return _type == Type.Literal ? "Literal" : _fragmentName;
    }

  }
}