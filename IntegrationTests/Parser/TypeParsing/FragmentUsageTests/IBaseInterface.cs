using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public interface IBaseInterface
  {
    void Foo ([Fragment ("ValidFragmentType")] int parameter1, string parameter2);
  }
}