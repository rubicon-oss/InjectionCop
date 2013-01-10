using System;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.TypeParsing
{
  public class OverridingMethodFragmentUsageRule : BaseFragmentUsageRule
  {
    public OverridingMethodFragmentUsageRule ()
        : base ("OverridingMethodFragmentUsageRule")
    {
    }

    public override void Parse (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);

      var overriddenMethod = method;
      while (overriddenMethod.OverriddenMethod != null)
        overriddenMethod = overriddenMethod.OverriddenMethod;

      if (overriddenMethod != method)
        MatchFragments(overriddenMethod, method);
    }
  }
}