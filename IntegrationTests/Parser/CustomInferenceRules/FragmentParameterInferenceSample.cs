// Copyright 2013 rubicon informationstechnologie gmbh
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace InjectionCop.IntegrationTests.Parser.CustomInferenceRules
{
  public class FragmentParameterInferenceSample: ParserSampleBase
  {
    public void ConcatWithLiterals ()
    {
      RequiresSqlFragment("literal" + "literal");
    }

    public void BinaryConcatEqualFragments ()
    {
      RequiresSqlFragment (SafeSource() + SafeSource());
    }

    public void BinaryConcatLiteralAndFragment ()
    {
      RequiresSqlFragment ("safe" + SafeSource());
    }

    public void BinaryConcatFragmentAndLiteral ()
    {
      RequiresSqlFragment (SafeSource() + "safe");
    }

    public void BinaryConcatDifferentFragments ()
    {
      RequiresSqlFragment (SafeSource() + ReturnsHtmlFragment());
    }

    public void BinaryConcatEqualFragmentVariables ()
    {
      string safe1 = SafeSource();
      string safe2 = SafeSource();
      RequiresSqlFragment (safe1 + safe2);
    }

    public void BinaryConcatDifferentFragmentVariables ()
    {
      string safe = SafeSource();
      string unsafeVariable = UnsafeSource();
      RequiresSqlFragment (safe+ unsafeVariable);
    }

    public void BinaryConcatEqualFragmentVariablesAcrossBlocks ()
    {
      string safe1 = SafeSource();
      string safe2 = SafeSource();
      if(safe1 == "dummy")
        RequiresSqlFragment (safe1 + safe2);
    }

    public void BinaryConcatDifferentFragmentVariablesAcrossBlocks ()
    {
      string safe1 = SafeSource();
      string safe2 = ReturnsHtmlFragment();
      if(safe1 == "dummy")
        RequiresSqlFragment (safe1 + safe2);
    }

    public void BinaryConcatSafeNestedCall ()
    {
      RequiresSqlFragment (SafeSourceRequiresSqlFragment (SafeSource() + SafeSource()) + "literal");
    }

    public void BinaryConcatUnsafeNestedCall ()
    {
      RequiresSqlFragment (SafeSourceRequiresSqlFragment (UnsafeSource() + SafeSource()) + "literal");
    }

    public void TernaryConcatSafeCall ()
    {
      RequiresSqlFragment ("literal" + SafeSource() + "literal");
    }

    public void TernaryConcatUnsafeCall ()
    {
      RequiresSqlFragment ("literal" + SafeSource() + UnsafeSource());
    }
    
    public void QuarternaryConcatSafeCall ()
    {
      RequiresSqlFragment ("literal" + SafeSource() + "literal" + SafeSource());
    }

    public void QuarternaryConcatUnsafeCall ()
    {
      RequiresSqlFragment ("literal" + SafeSource() + "literal" + UnsafeSource());
    }

    public void BinaryStringFormatSafeCall ()
    {
      RequiresSqlFragment (String.Format("{0}", "literal"));
    }

    public void BinaryStringFormatSafeCallUsingSqlFragment ()
    {
      RequiresSqlFragment (String.Format("{0}", SafeSource()));
    }

    public void BinaryStringFormatUnsafeCall ()
    {
      RequiresSqlFragment (String.Format("{0}", ReturnsHtmlFragment()));
    }

    public void TernaryStringFormatSafeCall ()
    {
      RequiresSqlFragment (String.Format("{0}", SafeSource()));
    }
    
    public void TernaryStringFormatUnsafeCall ()
    {
      RequiresSqlFragment (String.Format("{0}{1}", SafeSource(), ReturnsHtmlFragment()));
    }

    public void QuarternaryStringFormatSafeCall ()
    {
      RequiresSqlFragment (String.Format("{0}{1}{2}", "literal", SafeSource(), SafeSource()));
    }
    
    public void QuarternaryStringFormatUnsafeCall ()
    {
      RequiresSqlFragment (String.Format("{0}{1}{2}", SafeSource(), "literal", ReturnsHtmlFragment()));
    }

    public void SafeConcatCallWithArray ()
    {
      RequiresSqlFragment ("literal" + SafeSource() + "literal" + SafeSource() + "literal");
    }

    /*
     * pattern to support
    public void TernaryStringFormatSafeCall ()
    {
      var format = "{0}{1}";
      var strings = new[] { "literal", SafeSource() };
      strings[0]=UnsafeSource();
      RequiresSqlFragment (CreateString(format, strings));
    }

    private static string CreateString ([SqlFragment] string format, [SqlFragment] string[] strings)
    {
      return String.Format(format, strings);
    }
     * 
     */



  }
}
