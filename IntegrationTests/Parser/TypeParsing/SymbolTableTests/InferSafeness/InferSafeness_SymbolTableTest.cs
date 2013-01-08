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
using InjectionCop.Config;
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.SymbolTableTests.InferSafeness
{
  [TestFixture]
  public class InferSafeness_SymbolTableTest
  {
    private SymbolTable _symbolTable;

    [SetUp]
    public void SetUp ()
    {
      IBlacklistManager blacklistManager = new IDbCommandBlacklistManagerStub();
      _symbolTable = new SymbolTable (blacklistManager);
    }

    [Test]
    public void InferSafeness_NullSymbolName_ToleratedAndNoExceptionThrown ()
    {
      Method sample = TestHelper.GetSample<InferSafenessSample>("Foo");
      Block targetBlock = (Block) sample.Body.Statements[0];
      AssignmentStatement targetAssignment = (AssignmentStatement) targetBlock.Statements[1];
      Expression targetExpression = targetAssignment.Target;
      _symbolTable.InferSafeness (null, targetExpression);
    }
  }
}
