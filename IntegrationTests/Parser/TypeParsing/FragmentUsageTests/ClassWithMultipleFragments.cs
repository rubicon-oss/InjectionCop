using System;
using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.FragmentUsageTests
{
  public class ClassWithMultipleFragments
  {
    public void InvalidFragmentUsage (
        [Fragment ("FirstFragment")] [SqlFragment ()] int parameter)
    {
    }

    public void ValidFragmentUsage (
        [Fragment ("FirstFragment")] int parameter)
    {
    }

    public void NoFragmentUsage (
        int parameter)
    {
    }
  }
}