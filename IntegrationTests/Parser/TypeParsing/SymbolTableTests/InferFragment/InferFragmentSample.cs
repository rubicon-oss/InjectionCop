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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.SymbolTableTests.InferFragment
{
  class InferFragmentSample : ParserSampleBase
  {
    [Fragment("SampleFragment")]
    private new string _fragmentField = "dummy";

    private string _nonFragmentField = "dummy";

    public int AssignmentWithLiteral ()
    {
      int x = 3;
      return x;
    }

    public string AssignmentWithLocal ()
    {
      string x = "safe";
      DummyMethod (x);
      string y = x;
      return y;
    }

    public int AssignmentWithParameter (int parameter)
    {
      int x = parameter;
      return x;
    }

    public string AssignmentWithSafeMethodCall ()
    {
      string x = SafeSource();
      return x;
    }

    public string AssignmentWithUnsafeMethodCall ()
    {
      string x = UnsafeSource();
      return x;
    }

    public string AssignmentWithFragmentField ()
    {
      string x = _fragmentField;
      return x;
    }

    public string AssignmentWithNonFragmentField ()
    {
      string x = _nonFragmentField;
      return x;
    }

    public object AssignmentWithFragmentProperty ()
    {
      object x = PropertyWithFragment;
      return x;
    }

    [Fragment("PropertyFragmentType")]
    public object PropertyWithFragment { get; set; }

    public int PropertyWithoutFragment { get; set; }
  }
}
