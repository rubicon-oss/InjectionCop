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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Constructor
{
  public class ConstructorSample : ParserSampleBase
  {
    public ConstructorSample ()
    {
    }

    public ConstructorSample (string nonFragmentParameter)
    {
      DummyMethod (nonFragmentParameter);
    }

    public ConstructorSample (string nonFragmentParameter, [Fragment ("ConstructorFragment")] string fragmentParameter)
    {
      DummyMethod (nonFragmentParameter);
      RequiresConstructorFragment (fragmentParameter);
    }

    public ConstructorSample (string nonFragmentParameter, [Fragment ("ConstructorFragment")] string fragmentParameter, string dummy)
    {
      DummyMethod (nonFragmentParameter + fragmentParameter + dummy);
      RequiresConstructorFragment (nonFragmentParameter);
    }

    public ConstructorSample (string nonFragmentParameter, [Fragment ("ConstructorFragment")] string fragmentParameter, string dummy1, string dummy2)
      : this (nonFragmentParameter, fragmentParameter)
    {
      DummyMethod (nonFragmentParameter + fragmentParameter + dummy1 + dummy2);
    }

    public ConstructorSample (string nonFragmentParameter, [Fragment ("ConstructorFragment")] string fragmentParameter, string dummy1, string dummy2, string dummy3)
      : this (nonFragmentParameter, nonFragmentParameter, dummy1)
    {
      DummyMethod (nonFragmentParameter);
      DummyMethod (fragmentParameter);
      DummyMethod (dummy1);
      DummyMethod (dummy2);
      DummyMethod (dummy3);
    }

    public void RequiresConstructorFragment ([Fragment ("ConstructorFragment")] string fragmentParameter)
    {
      DummyMethod (fragmentParameter);
    }
  }
}
