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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Return
{
  class ReturnSample : ParserSampleBase
  {
    [Fragment ("DummyType")]
    private string _dummyTypeFragment = "safe";

    [return: Fragment ("DummyType")]
    public string ReturnFragmentMismatch ()
    {
      return UnsafeSource();
    }

    public string NoReturnAnnotation ()
    {
      return UnsafeSource();
    }

    [return: Fragment("ReturnFragmentType")]
    public int DeclarationWithReturn()
    {
      int i = 3;
      return i;
    }

    [return: Fragment ("DummyType")]
    public string ReturnsDummyType ()
    {
      return _dummyTypeFragment;
    }

    [return: Fragment ("DummyType")]
    public string ReturnsFieldWithWrongType ()
    {
      return _fragmentField;
    }
  }
}
