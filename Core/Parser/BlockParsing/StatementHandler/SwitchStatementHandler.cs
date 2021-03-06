﻿// Copyright 2013 rubicon informationstechnologie gmbh
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
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class SwitchStatementHandler : StatementHandlerBase<SwitchInstruction>
  {
    public SwitchStatementHandler (BlockParserContext blockParserContext)
        : base (blockParserContext)
    {
    }

    protected override void HandleStatement (HandleContext context)
    {
      SwitchInstruction switchInstruction = (SwitchInstruction) context.Statement;
      context.Successors.AddRange (switchInstruction.Targets.Select (caseBlock => caseBlock.UniqueKey));
    }
  }
}
