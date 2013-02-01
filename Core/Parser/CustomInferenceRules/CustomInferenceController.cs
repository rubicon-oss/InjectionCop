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
using System.Linq;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.CustomInferenceRules
{
  public class CustomInferenceController: ICustomInference
  {
    private readonly ICustomInference[] _inferenceRules;

    public CustomInferenceController ()
    {
      _inferenceRules = new ICustomInference[]
                        {
                            new FragmentParameterInference(),
                            new StringBuilderInference()
                        };
    }

    public bool Covers (Method method)
    {
      return _inferenceRules.Any (rule => rule.Covers (method));
    }

    public Fragment InferFragmentType (MethodCall methodCall, ISymbolTable context)
    {
      Fragment fragmentType = Fragment.CreateEmpty();
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      ICustomInference matchingRule = MatchingRule (calleeMethod);
      if (matchingRule != null)
      {
        fragmentType = matchingRule.InferFragmentType (methodCall, context);
      }
      return fragmentType;
    }

    public void PassProblem (
        MethodCall methodCall, List<IPreCondition> preConditions, ProblemMetadata problemMetadata, ISymbolTable symbolTable, IProblemPipe problemPipe)
    {
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      ICustomInference matchingRule = MatchingRule (calleeMethod);
      if (matchingRule != null)
      {
        matchingRule.PassProblem (methodCall, preConditions, problemMetadata, symbolTable, problemPipe);
      }
    }

    private ICustomInference MatchingRule (Method method)
    {
      if (Covers (method))
      {
        return _inferenceRules.Single (rule => rule.Covers (method));
      }
      else
      {
        return null;
      }
    }
  }
}
