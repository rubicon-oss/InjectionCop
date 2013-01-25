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
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.CustomInferenceRules
{
  public class FragmentParameterInference
  {
    private readonly List<string> _coveredMethods = new List<string>
                                                   {
                                                       "System.String.Concat(System.String,System.String)",
                                                       "System.String.Concat(System.String,System.String,System.String)",
                                                       "System.String.Concat(System.String,System.String,System.String,System.String)",
                                                       "System.String.Concat(System.String,System.String[])",
                                                       "System.String.Format(System.String,System.Object)",
                                                       "System.String.Format(System.String,System.Object,System.Object)",
                                                       "System.String.Format(System.String,System.Object,System.Object,System.Object)"
                                                   };

    public bool Covers (string methodFullname)
    {
      return _coveredMethods.Contains (methodFullname);
    }

    public void PassProblem (
        MethodCall methodCall, List<IPreCondition> preConditions, ProblemMetadata problemMetadata, ISymbolTable symbolTable, IProblemPipe problemPipe)
    {
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      if(_coveredMethods.Contains(calleeMethod.FullName))
      {
        foreach (var operand in methodCall.Operands)
        {
          string nestedVariableName;
          if (OperandIsVariableFromPrecedingBlock (operand, symbolTable, out nestedVariableName))
          {
            preConditions.Add (new AssignabilityPreCondition (nestedVariableName, problemMetadata.ExpectedFragment, problemMetadata));
          }
          else
          {
            problemPipe.AddProblem (problemMetadata);
          }
        }
      }
    }

    public Fragment InferMethodCallReturnFragmentType (MethodCall methodCall, ISymbolTable context)
    {
      Fragment returnFragment = Fragment.CreateEmpty();
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      if(_coveredMethods.Contains(calleeMethod.FullName))
      {
        returnFragment = ParameterFragmentIntersection (methodCall, context);
      }
      return returnFragment;
    }

    private bool OperandIsVariableFromPrecedingBlock (Expression operand, ISymbolTable symbolTable, out string variableName)
    {
      return IntrospectionUtility.IsVariable (operand, out variableName)
             && !symbolTable.Contains (variableName);
    }

    private Fragment ParameterFragmentIntersection (MethodCall methodCall, ISymbolTable context)
    {
      Fragment current = context.InferFragmentType (methodCall.Operands[0]);
      Fragment intersection = current;
      for (int i = 1; i < methodCall.Operands.Count; i++)
      {
        Fragment next = context.InferFragmentType (methodCall.Operands[i]);
        intersection = FragmentIntersection (current, next);
      }
      return intersection;
    }

    private Fragment FragmentIntersection (Fragment fragmentA, Fragment fragmentB)
    {
      bool fragmentAIsSuperior = fragmentA != fragmentB && fragmentA == Fragment.CreateLiteral();
      bool fragmentBIsSuperior = fragmentA != fragmentB && fragmentB == Fragment.CreateLiteral();
      
      if (fragmentA == fragmentB)
        return fragmentA;
      else if (fragmentAIsSuperior)
        return fragmentB;
      else if (fragmentBIsSuperior)
        return fragmentA;
      else
        return Fragment.CreateEmpty();
    }
  }
}
