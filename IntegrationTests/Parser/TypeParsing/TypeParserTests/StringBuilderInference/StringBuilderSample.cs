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
using System.Text;
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.StringBuilderInference
{
  public class StringBuilderSample : ParserSampleBase
  {
    public void SafeStringBuilderInference ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      RequiresBuilderFragment (stringBuilder);
    }

    public void UnsafeStringBuilderInference ()
    {
      StringBuilder stringBuilder = new StringBuilder (UnsafeSource());
      RequiresBuilderFragment (stringBuilder);
    }

    public void SafeStringBuilderReset ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      RequiresBuilderFragment (stringBuilder);
      stringBuilder = new StringBuilder ("literal");
      RequiresBuilderFragment (stringBuilder);
    }

    public void UnsafeStringBuilderReset ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      RequiresBuilderFragment (stringBuilder);
      stringBuilder = new StringBuilder (ReturnsHtmlFragment());
      RequiresBuilderFragment (stringBuilder);
    }

    public void SafeStringBuilderAcrossBlocks ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      if(SafeSource() == "dummy")
        RequiresBuilderFragment (stringBuilder);
    }

    public void UnsafeStringBuilderAcrossBlocks ()
    {
      StringBuilder stringBuilder = new StringBuilder (UnsafeSource());
      if(SafeSource() == "dummy")
        RequiresBuilderFragment (stringBuilder);
    }

    public void BooleanAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      stringBuilder.Append (true);
      RequiresBuilderFragment (stringBuilder);
    }

    public void DoubleAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      stringBuilder.Append (0.0d);
      RequiresBuilderFragment (stringBuilder);
    }

    public void SafeStringAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      stringBuilder.Append ("literal");
      RequiresBuilderFragment (stringBuilder);
    }

    public void UnsafeStringAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsBuilderFragment());
      stringBuilder.Append (UnsafeSource());
      RequiresBuilderFragment (stringBuilder);
    }

    public string RequiresBuilderFragment ([Fragment ("BuilderFragment")] StringBuilder stringBuilder)
    {
      return stringBuilder.ToString();
    }

    [FragmentGenerator]
    [return: Fragment("BuilderFragment")]
    private string ReturnsBuilderFragment ()
    {
      return "dummy";
    }
  }
}
