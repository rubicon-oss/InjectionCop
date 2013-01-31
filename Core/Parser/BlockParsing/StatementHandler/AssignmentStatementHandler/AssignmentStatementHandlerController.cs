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
using Microsoft.FxCop.Sdk;
using System.Linq;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class AssignmentStatementHandlerController : StatementHandlerBase<AssignmentStatement>
  {
    private readonly DefaultAssignmentStatementHandler _defaultAssignmentStatementHandler;
    private readonly AssignmentStatementHandlerBase[] _specificHandlers;

    public AssignmentStatementHandlerController (BlockParserContext blockParserContext)
        : base (blockParserContext)
    {
      _defaultAssignmentStatementHandler = new DefaultAssignmentStatementHandler (blockParserContext);
      _specificHandlers = new AssignmentStatementHandlerBase[]
      {
          new DelegateAssignmentStatementHandler (blockParserContext),
          new IndexerAssignmentStatementHandler (blockParserContext),
          new ArrayConstructStatementHandler (blockParserContext),
          new StringBuilderConstructStatementHandler(blockParserContext)
      };
    }

    protected override void HandleStatement (HandleContext context)
    {
      IStatementHandler selectedHandler;
      Func<AssignmentStatementHandlerBase, bool> handlerFilter =  handler => handler.Covers (context.Statement);
      if (_specificHandlers.Any (handlerFilter))
      {
        selectedHandler = _specificHandlers.Single (handlerFilter);
      }
      else
      {
        selectedHandler = _defaultAssignmentStatementHandler;
      }
      selectedHandler.Handle (context);
    }
  }
}
