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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Delegate
{
  public class DelegateSample : ParserSampleBase
  {
    public delegate string FragmentParameterDelegate ([Fragment ("DelegateFragmentType")] string fragmentParameter, string nonFragmentParameter);

    [return: Fragment("DelegateFragmentType")]
    public delegate string ReturnFragmentDelegate ();

    [return: Fragment("DelegateFragmentType")]
    public delegate string FragmentParameterAndReturnDelegate([Fragment("DelegateFragmentType")] string fragmentParameter, string nonFragmentParameter);

    private readonly FragmentParameterDelegate _fragmentParameterDelegate;

    public DelegateSample ()
    {
      _fragmentParameterDelegate = MatchingFragmentParameterDelegate;
    }

    private string MatchingFragmentParameterDelegate (string fragmentParameter, string nonFragmentParameter)
    {
      DummyMethod(nonFragmentParameter);
      return fragmentParameter;
    }

    private string MatchingFragmentParameterAndReturnDelegateSafeReturn(string fragmentParameter, string nonFragmentParameter)
    {
      DummyMethod(nonFragmentParameter);
      return fragmentParameter;
    }

    private string MatchingFragmentParameterAndReturnDelegateUnsafeReturn (string fragmentParameter, string nonFragmentParameter)
    {
      DummyMethod(fragmentParameter);
      return nonFragmentParameter;
    }
    
    private string SafeReturn ()
    {
      return "safe";
    }

    public string UnsafeReturn ()
    {
      return UnsafeSource();
    }

    public void SafeDelegateCall ()
    {
      FragmentParameterDelegate fragmentDelegate = MatchingFragmentParameterDelegate;
      fragmentDelegate ("safe", "safe");
    }

    public void UnsafeDelegateCall ()
    {
      FragmentParameterDelegate fragmentDelegate = MatchingFragmentParameterDelegate;
      fragmentDelegate (UnsafeSource(), "safe");
    }

    public void SafeDelegateCallUsingReturn ()
    {
      FragmentParameterDelegate fragmentParameterDelegate = MatchingFragmentParameterDelegate;
      ReturnFragmentDelegate returnFragmentDelegate = SafeReturn;
      fragmentParameterDelegate (returnFragmentDelegate(), "safe");
    }

    public void SafeDelegateFieldCall ()
    {
      _fragmentParameterDelegate ("safe", "safe");
    }

    public void UnsafeDelegateFieldCall ()
    {
      _fragmentParameterDelegate (UnsafeSource(), "safe");
    }

    public void DelegateWithSafeReturn()
    {
      FragmentParameterAndReturnDelegate sampleDelegate = MatchingFragmentParameterAndReturnDelegateSafeReturn;
      sampleDelegate("safe", "safe");
    }

    public void DelegateWithUnsafeReturn()
    {
      FragmentParameterAndReturnDelegate sampleDelegate = MatchingFragmentParameterAndReturnDelegateUnsafeReturn;
      sampleDelegate("safe", "safe");
    }
  }
}
