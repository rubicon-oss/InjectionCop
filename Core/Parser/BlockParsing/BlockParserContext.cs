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

namespace InjectionCop.Parser.BlockParsing
{
  public class BlockParserContext
  {
    private readonly IProblemPipe _problemPipe;
    private readonly Fragment _returnFragmentType;
    private readonly List<ReturnCondition> _returnConditions;
    private readonly IBlacklistManager _blacklistManager;
    private readonly BlockParser.InspectCallback _inspect;

    public BlockParserContext (IProblemPipe problemPipe, Fragment returnFragmentType, List<ReturnCondition> returnConditions, IBlacklistManager blacklistManager, BlockParser.InspectCallback inspect)
    {
      _problemPipe = problemPipe;
      _returnFragmentType = returnFragmentType;
      _returnConditions = returnConditions;
      _blacklistManager = blacklistManager;
      _inspect = inspect;
    }

    public IProblemPipe ProblemPipe
    {
      get { return _problemPipe; }
    }

    public Fragment ReturnFragmentType
    {
      get { return _returnFragmentType; }
    }

    public List<ReturnCondition> ReturnConditions
    {
      get { return _returnConditions; }
    }

    public IBlacklistManager BlacklistManager
    {
      get { return _blacklistManager; }
    }

    public BlockParser.InspectCallback Inspect
    {
      get { return _inspect; }
    }
  }
}
