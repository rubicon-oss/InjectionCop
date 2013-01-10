using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public class DerivedClassWithValidFragmentUsage : BaseClass
  {
    public void Foo ([Fragment("ValidFragmentType")] int parameter1, string parameter2)
    {
    }
  }
}