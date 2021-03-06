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
using System.Collections.Generic;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.CustomInferenceRules
{
  public interface ICustomInference
  {
    bool Infers (Method method);

    Fragment InferFragmentType (MethodCall methodCall, ISymbolTable context);

    bool Analyzes (Method method);
    void Analyze (MethodCall methodCall, ISymbolTable context, List<IPreCondition> preConditions);

    void PassProblem (
        MethodCall methodCall, List<IPreCondition> preConditions, ProblemMetadata problemMetadata, ISymbolTable symbolTable, IProblemPipe problemPipe);
  }
}
