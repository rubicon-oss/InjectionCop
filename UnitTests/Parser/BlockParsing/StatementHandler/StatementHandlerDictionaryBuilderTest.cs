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
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.BlockParsing.StatementHandler;
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser.BlockParsing.StatementHandler
{
  [TestFixture]
  public class StatementHandlerDictionaryBuilderTest
  {
    [Test]
    public void Build_ReturnsAllStatementHandlersNeededByBlockParser ()
    {
      MockRepository mocks = new MockRepository();
      IBlacklistManager blacklistManager = mocks.Stub<IBlacklistManager>();
      IProblemPipe problemPipe = mocks.Stub<IProblemPipe>();
      StatementHandlerDictionaryBuilder builder = new StatementHandlerDictionaryBuilder (
          blacklistManager, problemPipe, "returnFragmentType", new List<ReturnCondition>(), delegate (Expression expression) { });

      Dictionary<Type, IStatementHandler> handlers = builder.Build();
      bool assignmentStatementSupported = handlers.ContainsKey (typeof (AssignmentStatement));
      bool branchSupported = handlers.ContainsKey (typeof (Branch));
      bool expressionStatementSupported = handlers.ContainsKey (typeof (ExpressionStatement));
      bool returnNodeSupported = handlers.ContainsKey (typeof (ReturnNode));
      bool switchInstructionSupported = handlers.ContainsKey (typeof (SwitchInstruction));

      bool necessaryHandlersSupported = assignmentStatementSupported
                                        && branchSupported
                                        && expressionStatementSupported
                                        && returnNodeSupported
                                        && switchInstructionSupported;

      bool correctHandlerCount = handlers.Keys.Count == 5;

      Assert.That (necessaryHandlersSupported && correctHandlerCount, Is.True);
    }
  }
}
