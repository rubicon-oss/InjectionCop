// Copyright 2012 rubicon informationstechnologie gmbh
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
using InjectionCop.Fragment;
using InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AnonymousMethod;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Closure
{
  public class ClosureSample : ParserSampleBase
  {
    public delegate string FragmentParameterDelegate ();

    [Fragment ("ClosureFragmentType")]
    private string _safeField;

    private string _unsafeField;

    private ClosureSample ()
    {
      _safeField = "dummy";
      _unsafeField = "dummy";
    }

    public void SafeClosureUsingLocalVariable ()
    {
      string safeSource = "safe";
      AnonymousMethodSample.FragmentParameterDelegate fragmentDelegate =
          delegate { return RequiresClosureFragment (safeSource); };
      fragmentDelegate ("safe", "safe");
    }

    public void UnsafeClosureUsingLocalVariable ()
    {
      string safeSource = UnsafeSource();
      AnonymousMethodSample.FragmentParameterDelegate fragmentDelegate =
          delegate { return RequiresClosureFragment (safeSource); };
      fragmentDelegate ("safe", "safe");
    }

    public void SafeClosureUsingField ()
    {
      AnonymousMethodSample.FragmentParameterDelegate fragmentDelegate =
          delegate { return RequiresClosureFragment (_safeField);  };
      fragmentDelegate ("safe", "safe");
    }

    public void UnsafeClosureUsingField ()
    {
      AnonymousMethodSample.FragmentParameterDelegate fragmentDelegate =
          delegate { return RequiresClosureFragment (_unsafeField);  };
      fragmentDelegate ("safe", "safe");
    }
    
    private string RequiresClosureFragment ([Fragment ("ClosureFragmentType")] string fragmentParameter)
    {
      return fragmentParameter;
    }
  }
}
