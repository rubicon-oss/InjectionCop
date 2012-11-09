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
using InjectionCop.Attributes;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class MethodParser
  {
    private IBlackTypes _blackTypes;
    private TypeParser _typeParser;

    public MethodParser (IBlackTypes blackTypes, TypeParser typeParser)
    {
      _blackTypes = blackTypes;
      _typeParser = typeParser;
    }

    public void parse (Method method)
    {
      SymbolTable parameterSafeness = new SymbolTable (_blackTypes);
      FragmentAttribute sqlFragment = new FragmentAttribute ("SqlFragment");

      foreach (Parameter parameter in method.Parameters)
      {
        if (FragmentTools.Is (sqlFragment, parameter))
        {
          parameterSafeness.SetSafeness (parameter.Name, true);
        }
        else
        {
          parameterSafeness.SetSafeness (parameter.Name, false);
        }
      }

      foreach (Statement statement in method.Body.Statements)
      {
        Block methodBodyBlock = statement as Block;
        if (methodBodyBlock != null)
        {
          //BasicBlock basicBlock = new BasicBlock (methodBodyBlock, _blackTypes, _typeParser);
          // use basic block generator to build array of blocks möglicherweise mit stream

        }
      }
    }
  }
}
