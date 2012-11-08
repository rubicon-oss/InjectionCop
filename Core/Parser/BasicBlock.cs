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
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class BasicBlock
  {
    private Block _block;
    private IBlackTypes _blackTypes;
    private TypeParser _typeParser;

    public BasicBlock (Block block, IBlackTypes blackTypes, TypeParser typeParser)
    {
      _block = block;
      _blackTypes = blackTypes;
      _typeParser = typeParser;
    }
  }
}
