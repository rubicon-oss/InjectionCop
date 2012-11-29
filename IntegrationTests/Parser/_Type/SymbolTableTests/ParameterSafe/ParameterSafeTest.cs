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
using InjectionCop.Parser;
using InjectionCop.Parser._Block;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser._Type.SymbolTableTests.ParameterSafe
{
  [TestFixture]
  public class ParameterSafeTest
  {
    private ISymbolTable _symbolTable;

    [SetUp]
    public void SetUp ()
    {
      _symbolTable = new SymbolTable (new IDbCommandBlacklistManagerStub());
    }

    [Test]
    public void Parse_DeliverFragmentWhenNotExpected_IsSafe ()
    {
      Method sampleMethod = TestHelper.GetSample<ParameterSafeSample> ("DeliverFragmentWhenNotExpected");
      Block codeBlock = (Block) sampleMethod.Body.Statements[0];
      AssignmentStatement assignmentStatement = (AssignmentStatement) codeBlock.Statements[1];
      MethodCall sample = (MethodCall) assignmentStatement.Source;
      List<PreCondition> preconditions;
      bool isSafeCall = _symbolTable.ParametersSafe (sample, out preconditions);

      Assert.That (isSafeCall, Is.True);
    }

    [Test]
    public void Parse_CallWithoutFragments_IsSafe ()
    {
      Method sampleMethod = TestHelper.GetSample<ParameterSafeSample> ("CallWithoutFragments");
      Block codeBlock = (Block) sampleMethod.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement) codeBlock.Statements[1];
      UnaryExpression unaryExpression = (UnaryExpression) expressionStatement.Expression;
      MethodCall sample = (MethodCall) unaryExpression.Operand;
      List<PreCondition> preconditions;
      bool isSafeCall = _symbolTable.ParametersSafe (sample, out preconditions);

      Assert.That (isSafeCall, Is.True);
    }
  }
}
