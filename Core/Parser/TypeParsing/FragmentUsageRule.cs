using System;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.TypeParsing
{
  public class FragmentUsageRule : BaseFxCopRule, IProblemPipe
  {
    public FragmentUsageRule ()
        : base ("FragmentUsageRule")
    {
    }

    //public override ProblemCollection Check (TypeNode type)
    //{
    //  ArgumentUtility.CheckNotNull ("type", type);

    //  foreach (Member member in type.Members)
    //  {
    //    if (member is Method)
    //    {
    //      Method method = (Method) member;
    //      Parse (method);
    //    }
    //  }
    //  return Problems;
    //}

    public override ProblemCollection Check (Member member)
    {
      if (member is Method)
      {
        var method = (Method) member;
        Parse (method);
      }
      return Problems;
    }

    public void Parse (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);

      var overriddenMethod = method;
      while (overriddenMethod.OverriddenMethod != null)
        overriddenMethod = overriddenMethod.OverriddenMethod;

      if (overriddenMethod != method)
        MatchFragments(overriddenMethod, method);

      //var interfaceCollection = method.DeclaringType.Interfaces;
      //var interfaces = overriddenMethod.DeclaringType.Interfaces;


      //if (method.ImplementedInterfaceMethods != null)
      //  foreach (var interfaceMethod in method.ImplementedInterfaceMethods)
      //    MatchFragments (method, interfaceMethod);
    }

    private void MatchFragments (Method baseMethod, Method overridingMethod)
    {
      foreach (var parameter in baseMethod.Parameters)
      {
        var fragmentType = FragmentUtility.GetFragmentType (parameter.Attributes);

        if (fragmentType != SymbolTable.EMPTY_FRAGMENT)
        {
          var overriddenParameter = overridingMethod.Parameters[parameter.ParameterListIndex];
          var overriddenFragmentType = FragmentUtility.GetFragmentType (overriddenParameter.Attributes);

          if (overriddenFragmentType != fragmentType && overriddenFragmentType != SymbolTable.EMPTY_FRAGMENT)
            AddProblem (new ProblemMetadata (overridingMethod.UniqueKey, overridingMethod.SourceContext, fragmentType, overriddenFragmentType));
        }
      }
    }

    public void AddProblem (ProblemMetadata problemMetadata)
    {
      ArgumentUtility.CheckNotNull ("problemMetadata", problemMetadata);
      var resolution = GetResolution (problemMetadata.ExpectedFragment, problemMetadata.GivenFragment);
      var problem = new Problem (resolution, problemMetadata.SourceContext, CheckId);
      Problems.Add (problem);
    }
  }
}