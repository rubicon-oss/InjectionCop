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
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;

namespace InjectionCop.Parser.BlockParsing.PreCondition
{
  public class CustomInferencePreCondition : PreConditionBase
  {
    public CustomInferencePreCondition (string symbol, Fragment fragment)
        : base (symbol, fragment)
    {
    }

    public CustomInferencePreCondition (string symbol, Fragment fragment, ProblemMetadata problemMetadata)
        : base (symbol, fragment, problemMetadata)
    {
    }

    protected override bool ViolationCheckStrategy (ISymbolTable context)
    {
      Fragment givenFragment = context.GetFragmentType (_symbol);
      return !FragmentUtility.FragmentTypesAssignable (givenFragment, _fragment);
    }

    public override void HandleViolation (ISymbolTable context, IProblemPipe problemPipe)
    {
      if (!IsViolated (context))
        return;

      if (ProblemMetadata != null)
        problemPipe.AddProblem (_problemMetadata);
      
      context.MakeUnsafe (_symbol);
    }
  }
}
