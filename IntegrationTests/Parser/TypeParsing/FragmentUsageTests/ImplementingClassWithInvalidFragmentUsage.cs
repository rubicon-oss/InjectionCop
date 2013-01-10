using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public class ImplementingClassWithInvalidFragmentUsage : IBaseInterface
  {
    public void Foo ([Fragment("InvalidFragmentType")]int parameter1, string parameter2)
    {
    }
  }
}