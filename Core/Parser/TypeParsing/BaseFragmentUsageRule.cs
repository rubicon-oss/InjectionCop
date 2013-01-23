using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.TypeParsing
{
  public abstract class BaseFragmentUsageRule : BaseFxCopRule, IProblemPipe
  {
    protected BaseFragmentUsageRule (string ruleName)
        : base(ruleName)
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

    public abstract void Parse (Method method);

    protected void MatchFragments (Method baseMethod, Method overridingMethod)
    {
      foreach (var parameter in baseMethod.Parameters)
      {
        var fragmentType = FragmentUtility.GetFragmentType (parameter.Attributes);

        if (fragmentType != Fragment.CreateEmpty())
        {
          var overriddenParameter = overridingMethod.Parameters[parameter.ParameterListIndex];
          var overriddenFragmentType = FragmentUtility.GetFragmentType (overriddenParameter.Attributes);

          if (overriddenFragmentType != fragmentType && overriddenFragmentType != Fragment.CreateEmpty())
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