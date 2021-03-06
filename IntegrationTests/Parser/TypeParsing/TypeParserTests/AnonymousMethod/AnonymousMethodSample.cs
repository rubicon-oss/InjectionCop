﻿// Copyright 2012 rubicon informationstechnologie gmbh
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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AnonymousMethod
{
  class AnonymousMethodSample: ParserSampleBase
  {
    public delegate string FragmentParameterDelegate ([Fragment ("AnonymousMethodFragmentType")] string fragmentParameter, string nonFragmentParameter);

    [return: Fragment("AnonymousMethodFragmentType")]
    public delegate string ReturnFragmentDelegate ();

    public void SafeAnonymousMethodCall ()
    {
      FragmentParameterDelegate fragmentDelegate =
          delegate (string fragmentParameter, string nonFragmentParameter) { return fragmentParameter + nonFragmentParameter; };
      fragmentDelegate ("safe", "safe");
    }

    public void UnsafeAnonymousMethodCall ()
    {
      FragmentParameterDelegate fragmentDelegate = 
        delegate (string fragmentParameter, string nonFragmentParameter) { return fragmentParameter + nonFragmentParameter; };
      fragmentDelegate (UnsafeSource(), "safe");
    }

    public void SafeAnonymousMethodCallUsingReturn ()
    {
      FragmentParameterDelegate fragmentParameterDelegate =
          delegate (string fragmentParameter, string nonFragmentParameter) { return fragmentParameter + nonFragmentParameter; };
      ReturnFragmentDelegate returnFragmentDelegate =
          delegate { return "safe"; };
      fragmentParameterDelegate (returnFragmentDelegate(), "safe");
    }

    public void SafeMethodCallInsideAnonymousMethod ()
    {
      FragmentParameterDelegate fragmentDelegate =
          delegate (string fragmentParameter, string nonFragmentParameter)
          {
            RequiresAnonymousMethodFragment (fragmentParameter); 
            return fragmentParameter + nonFragmentParameter; 
          };
      fragmentDelegate ("safe", "safe");
    }

    public void UnsafeMethodCallInsideAnonymousMethod ()
    {
      FragmentParameterDelegate fragmentDelegate =
          delegate (string fragmentParameter, string nonFragmentParameter)
          {
            RequiresAnonymousMethodFragment (nonFragmentParameter); 
            return fragmentParameter + nonFragmentParameter; 
          };
      fragmentDelegate ("safe", "safe");
    }

    public void SafeReturnInsideAnonymousMethod()
    {
      ReturnFragmentDelegate returnFragmentDelegate =
        delegate
        {
          return "safe";
        };
      returnFragmentDelegate();
    }

    public void UnsafeReturnInsideAnonymousMethod()
    {
      ReturnFragmentDelegate returnFragmentDelegate =
        delegate
        {
          return UnsafeSource();
        };
      returnFragmentDelegate();
    }

    private void RequiresAnonymousMethodFragment([Fragment("AnonymousMethodFragmentType")] string fragmentParameter)
    {
      DummyMethod (fragmentParameter);
    }
  }
}
