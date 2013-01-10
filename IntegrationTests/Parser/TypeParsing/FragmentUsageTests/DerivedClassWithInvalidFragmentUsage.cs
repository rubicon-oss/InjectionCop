using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public class DerivedClassWithInvalidFragmentUsage : BaseClass
  {
    public override void Foo ([Fragment("InvalidFragmentType")]int parameter1, string parameter2)
    {
    }

    public override void MethodWithFragmentOnSecondParameter (int parameter1, [Fragment("InvalidFragmentTypeOnSecondParameter")] int parameter2)
    {
    }
  }
}