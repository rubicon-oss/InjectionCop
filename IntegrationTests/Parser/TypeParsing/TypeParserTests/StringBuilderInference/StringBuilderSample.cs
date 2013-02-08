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
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void UnsafeStringBuilderInference ()
    {
      StringBuilder stringBuilder = new StringBuilder (UnsafeSource());
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void SafeStringBuilderReset ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      RequiresSafeFragmentBuilder (stringBuilder);
      stringBuilder = new StringBuilder ("literal");
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void UnsafeStringBuilderReset ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      RequiresSafeFragmentBuilder (stringBuilder);
      stringBuilder = new StringBuilder (ReturnsHtmlFragment());
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void SafeStringBuilderAcrossBlocks ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      if(SafeSource() == "dummy")
        RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void UnsafeStringBuilderAcrossBlocks ()
    {
      StringBuilder stringBuilder = new StringBuilder (UnsafeSource());
      if(SafeSource() == "dummy")
        RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void BooleanAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      stringBuilder.Append (true);
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void DoubleAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      stringBuilder.Append (0.0d);
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void SafeStringAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      stringBuilder.Append ("literal");
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void UnsafeStringAppend ()
    {
      StringBuilder stringBuilder = new StringBuilder (ReturnsSafeFragment());
      stringBuilder.Append (UnsafeSource());
      RequiresSafeFragmentBuilder (stringBuilder);
    }

    public void AppendingLiteralDoesNotChangeSafeFragmentType()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsSafeFragment());
      stringBuilder.Append("literal");
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append("literal");
      }
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void AppendingLiteralDoesNotChangeUnsafeFragmentType()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsHtmlFragment());
      stringBuilder.Append("literal");
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append("literal");
      }
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void SafeStringAppendAcrossBlocks()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsSafeFragment());
      stringBuilder.Append("literal");
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append("literal");
      }
      else
      {
        stringBuilder.Append(ReturnsSafeFragment());
      }
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void UnsafeStringAppendAcrossBlocks()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsSafeFragment());
      stringBuilder.Append("literal");
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append("literal");
      }
      else
      {
        stringBuilder.Append(ReturnsHtmlFragment());
      }
      RequiresSafeFragmentBuilder(stringBuilder);
    }
    
    public void SafeCallsInDifferentBlocks()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsSafeFragment());
      stringBuilder.Append("literal");
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append(ReturnsSafeFragment());
        RequiresSafeFragmentBuilder (stringBuilder);
      }
      
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void StringBuilderBlockConflict()
    {
      StringBuilder stringBuilder = new StringBuilder(ReturnsSafeFragment());
      stringBuilder.Append("literal");
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append(ReturnsHtmlFragment());
        RequiresHtmlBuilder (stringBuilder);
      }
      
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void SafeFragmentTypeDefinitionAcrossBlocks ()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append(ReturnsSafeFragment());
      }
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void UnsafeFragmentTypeDefinitionAcrossBlocks ()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (ReturnsSafeFragment() == "dummy")
      {
        stringBuilder.Append(ReturnsHtmlFragment());
      }
      RequiresSafeFragmentBuilder(stringBuilder);
    }
    
    public void UndefinedInitializationSetSafe ()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(ReturnsSafeFragment());
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void UndefinedInitializationSetUnsafe ()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(ReturnsHtmlFragment());
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void UnsafeAppendChain ()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append (UnsafeSource());
      stringBuilder.Append(ReturnsHtmlFragment());
      stringBuilder.Append (ReturnsHtmlFragment());
      RequiresSafeFragmentBuilder(stringBuilder);
    }

    public void SafeStringBuilderToString ()
    {
      StringBuilder builder = new StringBuilder(ReturnsSafeFragment());
      RequiresSafeFragment (builder.ToString());
    }

    public string RequiresSafeFragmentBuilder ([Fragment ("BuilderFragment")] StringBuilder stringBuilder)
    {
      return stringBuilder.ToString();
    }

    public string RequiresHtmlBuilder ([Fragment ("HtmlFragment")] StringBuilder stringBuilder)
    {
      return stringBuilder.ToString();
    }

    public string RequiresSafeFragment ([Fragment ("BuilderFragment")] string parameter)
    {
      return parameter;
    }
    
    [FragmentGenerator]
    [return: Fragment("BuilderFragment")]
    private string ReturnsSafeFragment ()
    {
      return "dummy";
    }
  }
}
