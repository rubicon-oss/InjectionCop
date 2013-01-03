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
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.MethodParsing.MethodGraphTests
{
  [TestFixture]
  public class Interface_TypeParserTest : MethodGraph_TestBase
  {
    [Test]
    public void IsEmpty_MethodNonAnnotated_ReturnsTrue ()
    {
      TypeNode objectTypeNode = IntrospectionUtility.TypeNodeFactory (typeof (object));
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory (typeof (string));
      Method sampleMethod = IntrospectionUtility.MethodFactory (typeof (InterfaceSample), "MethodNonAnnotated", objectTypeNode, stringTypeNode);
      IMethodGraph methodGraph = BuildMethodGraph(sampleMethod);
      //Assert.That (methodGraph.IsEmpty(), Is.True);
      Assert.That (methodGraph, Is.Null);
    }
  }
}
