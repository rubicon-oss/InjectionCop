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
  public class ArrayConstructStatementHandler: AssignmentStatementHandlerBase
  {
    public ArrayConstructStatementHandler (BlockParserContext blockParserContext)
        : base (blockParserContext)
    {
    }

    protected override void HandleStatement (HandleContext context)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) context.Statement;
      if (!CoversAssignment (assignmentStatement))
        return;

      var target = (Local) assignmentStatement.Target;
      context.ArrayFragmentTypeDefined[target.Name.Name] = false;
      context.AssignmentTargetVariables.Add (target.Name.Name);
    }

    protected override bool CoversAssignment (AssignmentStatement assignmentStatement)
    {
      return assignmentStatement.Target is Local && assignmentStatement.Source is ConstructArray;
    }
  }
}
