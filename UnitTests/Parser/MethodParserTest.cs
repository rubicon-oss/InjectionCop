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
using System.Collections.Generic;
using InjectionCop.Config;
using InjectionCop.IntegrationTests.Parser;
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser
{
  [TestFixture]
  public class MethodParserTest
  {
    private SymbolTable _preConditions;
    private MethodParser _methodParser;
    private IBlackTypes _blackTypes;

    [SetUp]
    public void SetUp ()
    {
      _blackTypes = new IDbCommandBlackTypesStub();
      _preConditions = new SymbolTable (_blackTypes);
      _preConditions.SetSafeness ("x", true);
      _preConditions.SetSafeness ("y", false);
      TypeParser typeParser = new TypeParser (_blackTypes);
      _methodParser = new MethodParser (_blackTypes, typeParser);
    }

    [Test]
    public void Parse_EmptyGraph_NoProblems ()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();
      using(mocks.Record())
      {
        SetupResult.For (methodGraph.InitialBlockId)
            .Throw (new InjectionCopException("Graph is empty"));
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _preConditions);
      Assert.That (result.Count, Is.EqualTo (0));
    }

    [Test]
    public void Parse_SingleNodeNoPrecondition_NoProblems()
    {
      MockRepository mocks = new MockRepository();
      IMethodGraph methodGraph = mocks.Stub<IMethodGraph>();
      string[] preConditions = (new List<string>()).ToArray();
      SymbolTable postConditions = new SymbolTable (_blackTypes);
      int[] successors = (new List<int>()).ToArray();
      BasicBlock node = new BasicBlock (0, preConditions, postConditions, successors);
      using(mocks.Record())
      {
        SetupResult.For (methodGraph.InitialBlockId)
            .Return(0);

        methodGraph.GetBasicBlockById (0);
        LastCall.Return(node);
      }
      ProblemCollection result = _methodParser.Parse (methodGraph, _preConditions);
      Assert.That (result.Count, Is.EqualTo (0));
    }

    [Test]
    public void Parse_SingleNodePreconditionViolated_ReturnsProblem()
    {
      Assert.Fail ("implement me :)");
    }
  }
}
