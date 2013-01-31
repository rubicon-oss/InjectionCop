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
using InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class StatementHandlerDictionaryBuilder
  {
    private readonly BlockParserContext _blockParserContext;

    public StatementHandlerDictionaryBuilder (BlockParserContext blockParserContext)
    {
      _blockParserContext = blockParserContext;
    }

    public Dictionary<Type, IStatementHandler> Build ()
    {
      Dictionary<Type, IStatementHandler> statementHandlers = new Dictionary<Type, IStatementHandler>();

      ReturnStatementHandler returnStatementHandler = new ReturnStatementHandler (_blockParserContext);
      AssignmentStatementHandlerController assignmentStatementHandler = new AssignmentStatementHandlerController (_blockParserContext);
      ExpressionStatementHandler expressionStatementHandler = new ExpressionStatementHandler (_blockParserContext);
      BranchStatementHandler branchStatementHandler = new BranchStatementHandler (_blockParserContext);
      SwitchStatementHandler switchStatementHandler = new SwitchStatementHandler (_blockParserContext);

      statementHandlers[returnStatementHandler.HandledStatementType] = returnStatementHandler;
      statementHandlers[assignmentStatementHandler.HandledStatementType] = assignmentStatementHandler;
      statementHandlers[expressionStatementHandler.HandledStatementType] = expressionStatementHandler;
      statementHandlers[branchStatementHandler.HandledStatementType] = branchStatementHandler;
      statementHandlers[switchStatementHandler.HandledStatementType] = switchStatementHandler;

      return statementHandlers;
    }
  }
}
