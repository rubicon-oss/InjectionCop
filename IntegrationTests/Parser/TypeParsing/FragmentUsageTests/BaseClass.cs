using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public class BaseClass
  {
    public virtual void Foo ([Fragment("ValidFragmentType")] int parameter1, string parameter2)
    {
    }

    public virtual void MethodWithFragmentOnSecondParameter (int parameter1, [Fragment ("ValidFragmentType")] int parameter2)
    {
    }
  }
}