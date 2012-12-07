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
using System.Linq;
using InjectionCop.Parser;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.IntegrationTests.Parser
{
  public class TestHelper
  {
    public static bool ContainsProblemID (string id, ProblemCollection result)
    {
      return result.Any (problem => problem.Id == id);
    }

    public static Method GetSample<SampleClass>(string methodName, params TypeNode[] methodParameters)
    {
      Method sample = IntrospectionUtility.MethodFactory<SampleClass> (methodName, methodParameters);
      return sample;
    }

    public static Method GetSample (Type targetType, string methodName, params TypeNode[] methodParameters)
    {
      Method sample = IntrospectionUtility.MethodFactory(targetType, methodName, methodParameters);
      return sample;
    }

  }
}
