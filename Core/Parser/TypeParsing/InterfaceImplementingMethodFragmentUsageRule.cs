using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.TypeParsing
{
  public class InterfaceImplementingMethodFragmentUsageRule : BaseFragmentUsageRule
  {
    
    public InterfaceImplementingMethodFragmentUsageRule ()
        : base ("InterfaceImplementingMethodFragmentUsageRule")
    {
    }

    public override void Parse (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);

      foreach (var interfaceDeclaration in IntrospectionUtility.InterfaceDeclarations(method))
        MatchFragments(interfaceDeclaration, method);
    } 
  }
}