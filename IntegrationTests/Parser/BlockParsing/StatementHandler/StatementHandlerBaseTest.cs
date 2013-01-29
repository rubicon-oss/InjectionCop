// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.BlockParsing.StatementHandler;
using InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.IntegrationTests.Parser.BlockParsing.StatementHandler
{
  [TestFixture]
  public class StatementHandlerBaseTest
  {
    [Test]
    [ExpectedException(typeof(InjectionCopException), ExpectedMessage = "Expected to handle AssignmentStatement but got ReturnNode")]
    public void Handle_WrongStatementType_ThrowsException ()
    {
      MockRepository mocks = new MockRepository();
      IBlacklistManager blacklistManager = mocks.Stub<IBlacklistManager>();
      StatementHandlerBase<AssignmentStatement> handler = new AssignmentStatementHandlerController (
          new ProblemPipeStub(), Fragment.CreateNamed ("returnFragmentType"), new List<ReturnCondition>(), blacklistManager, delegate { });
      Method sampleMethod = IntrospectionUtility.MethodFactory<StatementHandlerBaseSample> ("ContainsReturnStatement");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[1];
      Statement sample = sampleBlock.Statements[0];

      ISymbolTable symbolTable = mocks.Stub<ISymbolTable>();
      handler.Handle (sample, symbolTable, new List<IPreCondition>(), new List<string>(), new List<BlockAssignment>(), new List<int>(), new Dictionary<string, bool>());
    }
  }
}
