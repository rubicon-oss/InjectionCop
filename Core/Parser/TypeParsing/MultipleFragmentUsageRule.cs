using System;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using System.Linq;

namespace InjectionCop.Parser.TypeParsing
{
  public class MultipleFragmentUsageRule : BaseFxCopRule
  {
    public MultipleFragmentUsageRule ()
        : base ("MulitpleFragmentUsageRule")
    {
    }

    public override ProblemCollection Check (Member member)
    {
      if (member is Method)
      {
        var method = (Method) member;
        Parse (method);
      }
      return Problems;
    }

    private void Parse (Method method)
    {
      foreach (var parameter in method.Parameters)
      {
        var fragmentCount = parameter.Attributes.Count(FragmentUtility.IsFragment);

        if (fragmentCount > 1)
          AddProblem (parameter.SourceContext, parameter.Name);
      }
    }

    private void AddProblem (SourceContext sourceContext, Identifier parameterName)
    {
      var resolution = GetResolution (parameterName.Name);
      var problem = new Problem (resolution, sourceContext, CheckId);
      Problems.Add (problem);
    }
  }
}