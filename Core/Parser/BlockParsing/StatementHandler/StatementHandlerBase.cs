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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public abstract class StatementHandlerBase<T> : IStatementHandler
      where T : Statement
  {
    protected readonly IProblemPipe _problemPipe;
    protected readonly Fragment _returnFragmentType;
    protected readonly List<ReturnCondition> _returnConditions;
    protected readonly IBlacklistManager _blacklistManager;
    protected BlockParser.InspectCallback _inspect;

    protected StatementHandlerBase (
        IProblemPipe problemPipe,
        Fragment returnFragmentType,
        List<ReturnCondition> returnConditions,
        IBlacklistManager blacklistManager,
        BlockParser.InspectCallback inspect)
    {
      _problemPipe = problemPipe;
      _returnFragmentType = returnFragmentType;
      _returnConditions = returnConditions;
      _blacklistManager = blacklistManager;
      _inspect = inspect;
    }

    public Type HandledStatementType
    {
      get { return typeof (T); }
    }

    public void Handle (HandleContext context)
    {
      if (context.Statement is T)
      {
        HandleStatement (context);
      }
      else
      {
        throw new InjectionCopException ("Expected to handle " + typeof (T).Name + " but got " + context.Statement.GetType().Name);
      }
    }

    protected abstract void HandleStatement (HandleContext context);
  }
}


