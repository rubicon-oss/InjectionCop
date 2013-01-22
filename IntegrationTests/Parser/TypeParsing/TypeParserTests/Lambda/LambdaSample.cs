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
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Lambda
{
  class LambdaSample: ParserSampleBase
  {
    public delegate string FragmentParameterDelegate ([Fragment ("LambdaFragmentType")] string fragmentParameter, string nonFragmentParameter);

    [return: Fragment("LambdaFragmentType")]
    public delegate string ReturnFragmentDelegate ();

    public void SafeLambdaCall ()
    {
      FragmentParameterDelegate fragmentDelegate =
          (fragmentParameter, nonFragmentParameter) => fragmentParameter + nonFragmentParameter;
          
      fragmentDelegate ("safe", "safe");
    }

    public void UnsafeLambdaCall ()
    {
      FragmentParameterDelegate fragmentDelegate =
          (fragmentParameter, nonFragmentParameter) => fragmentParameter + nonFragmentParameter;
      fragmentDelegate (UnsafeSource(), "safe");
    }

    public void SafeLambdaCallUsingReturn ()
    {
      FragmentParameterDelegate fragmentParameterDelegate =
          (fragmentParameter, nonFragmentParameter) => fragmentParameter + nonFragmentParameter;
      ReturnFragmentDelegate returnFragmentDelegate =
          () => "safe";
      fragmentParameterDelegate (returnFragmentDelegate(), "safe");
    }

    public void SafeMethodCallInsideLambda()
    {
      FragmentParameterDelegate fragmentDelegate =
          (fragmentParameter, nonFragmentParameter) => { RequiresLambdaFragment(fragmentParameter); return fragmentParameter + nonFragmentParameter; };
      fragmentDelegate ("dummy", "dummy");
    }

    public void UnsafeMethodCallInsideLambda()
    {
      FragmentParameterDelegate fragmentDelegate =
          (fragmentParameter, nonFragmentParameter) => { RequiresLambdaFragment(nonFragmentParameter); return fragmentParameter + nonFragmentParameter; };
      fragmentDelegate ("dummy", "dummy");
    }

    public void SafeReturnInsideLambda()
    {
      ReturnFragmentDelegate returnFragmentDelegate = () => "safe";
      returnFragmentDelegate();
    }

    public void UnsafeReturnInsideLambda()
    {
      ReturnFragmentDelegate returnFragmentDelegate = () => UnsafeSource();
      returnFragmentDelegate();
    }

    private void RequiresLambdaFragment([Fragment("LambdaFragmentType")] string fragmentParameter)
    {
      DummyMethod(fragmentParameter);
    }
  }
}
