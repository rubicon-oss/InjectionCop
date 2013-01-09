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
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.SymbolTableTests.BlacklistManagerIntegration
{
  [TestFixture]
  public class BlacklistManagerIntegration_SymbolTableTest
  {
    private ISymbolTable _symbolTable;

    [SetUp]
    public void SetUp ()
    {
      IBlacklistManager blacklistManager = new IDbCommandBlacklistManagerStub();
      _symbolTable = new SymbolTable (blacklistManager);
    }

    [Test]
    public void ParametersSafe_UnsafeBlacklistedCall_ReturnsFalse_()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("UnsafeBlacklistedCall");
      Block codeBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)codeBlock.Statements[2];
      MethodCall methodCall = (MethodCall) expressionStatement.Expression;

      List<AssignabilityPreCondition> preconditions;
      List<ProblemMetadata> parameterProblems;
      _symbolTable.ParametersSafe (methodCall, out preconditions, out parameterProblems);
      Assert.That(parameterProblems.Count, Is.Not.EqualTo(0));
    }
    
    [Test]
    public void ParametersSafe_SafeBlacklistedCall_ReturnsFalse_()
    {
      Method sample = TestHelper.GetSample<BlacklistManagerIntegrationSample>("SafeBlacklistedCall");
      Block codeBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)codeBlock.Statements[2];
      MethodCall methodCall = (MethodCall) expressionStatement.Expression;

      List<AssignabilityPreCondition> preconditions;
      List<ProblemMetadata> parameterProblems;
      _symbolTable.ParametersSafe (methodCall, out preconditions, out parameterProblems);
      Assert.That(parameterProblems.Count, Is.EqualTo(0));
    }
    
  }
}
