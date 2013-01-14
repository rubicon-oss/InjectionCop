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

namespace InjectionCop.IntegrationTests.Utilities
{
  public class IntrospectionUtility_ClassSample
  {
    private string _field = "dummy";

    public void UsingField ()
    {
      Dummy (_field);
    }

    public void FieldAssignment ()
    {
      _field = "dummy";
    }

    public string NonFieldAssignment ()
    {
      string x = Dummy ("dummy");
      return x;
    }

    public string Dummy (string parameter)
    {
      return parameter;
    }

    public object AnyProperty { get; set; }

    public string get_NonExistingProperty ()
    {
      NestedClass nested = new NestedClass();
      return "dummy" + nested;
    }

    public string get_NonExistingProperty (string parameter)
    {
      return parameter;
    }

    public void set_NonExistingProperty ()
    {
    }

    private class NestedClass
    {
    }

    public object[] ArrayVariableAndIndexer ()
    {
      object[] objectArray = new object[5];
      objectArray[0] = new object();
      return objectArray;
    }

    public delegate string Closure ();
    
    public string UsingClosure ()
    {
      string environmentVariable = "environment";
      Closure closure = () => environmentVariable;
      return closure();
    }

  }

}