using System;

namespace InjectionCop.Config
{
  public class FragmentSignature
  {
    private readonly bool _isGenerator;
    private readonly string[] _parameterFragmentTypes;
    private readonly string _returnFragmentType;

    public FragmentSignature (string[] parameterFragmentTypes, string returnFragmentType, bool isGenerator)
    {
      _parameterFragmentTypes = parameterFragmentTypes;
      _returnFragmentType = returnFragmentType;
      _isGenerator = isGenerator;
    }

    public string[] ParameterFragmentTypes
    {
      get { return _parameterFragmentTypes; }
    }

    public string ReturnFragmentType
    {
      get { return _returnFragmentType; }
    }

    public bool IsGenerator
    {
      get { return _isGenerator; }
    }
  }
}