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

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class AssignmentStatementHandlerController : StatementHandlerBase<AssignmentStatement>
  {
    private readonly DefaultAssignmentStatementHandler _defaultAssignmentStatementHandler;
    private readonly DelegateAssignmentStatementHandler _delegateAssignmentStatementHandler;
    private readonly IndexerAssignmentStatementHandler _indexerAssignmentStatementHandler;
    private readonly ArrayConstructStatementHandler _arrayConstructStatementHandler;

    public AssignmentStatementHandlerController (BlockParserContext blockParserContext)
        : base (blockParserContext)
    {
      _defaultAssignmentStatementHandler = new DefaultAssignmentStatementHandler (blockParserContext);
      _delegateAssignmentStatementHandler = new DelegateAssignmentStatementHandler (blockParserContext);
      _indexerAssignmentStatementHandler = new IndexerAssignmentStatementHandler (blockParserContext);
      _arrayConstructStatementHandler = new ArrayConstructStatementHandler (blockParserContext);
    }

    protected override void HandleStatement (HandleContext context)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) context.Statement;
      bool sourceIsDelegate = assignmentStatement.Source.NodeType == NodeType.Construct
                              && assignmentStatement.Source.Type.NodeType == NodeType.DelegateNode;
      bool arrayInitialization = assignmentStatement.Source.NodeType == NodeType.ConstructArray
                                 && assignmentStatement.Target.NodeType == NodeType.Local;

      if (sourceIsDelegate)
      {
        _delegateAssignmentStatementHandler.Handle (context);
      }
      else if (arrayInitialization)
      {
        _arrayConstructStatementHandler.Handle (context);
      }
      else if (assignmentStatement.Target is Indexer)
      {
        _indexerAssignmentStatementHandler.Handle (context);
      }
      else
      {
        _defaultAssignmentStatementHandler.Handle (context);
      }
    }
  }
}
