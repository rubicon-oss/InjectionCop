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

namespace InjectionCop.Parser.BlockParsing
{
  public class MethodCallAnalyzer
  {
    private readonly IProblemPipe _problemPipe;

    private ISymbolTable _symbolTable;
    private List<IPreCondition> _preConditions;

    public MethodCallAnalyzer (IProblemPipe problemPipe)
    {
      _problemPipe = problemPipe;
    }

    public void Analyze (MethodCall methodCall, ISymbolTable symbolTable, List<IPreCondition> preConditions)
    {
      _symbolTable = symbolTable;
      _preConditions = preConditions;
      CheckParameters (methodCall);
      UpdateOutAndRefSymbols (methodCall);
    }

    private void CheckParameters (MethodCall methodCall)
    {
      List<IPreCondition> additionalPreConditions;
      List<ProblemMetadata> parameterProblems;
      ParametersSafe (methodCall, out additionalPreConditions, out parameterProblems);
      parameterProblems.ForEach (parameterProblem => _problemPipe.AddProblem (parameterProblem));
      _preConditions.AddRange (additionalPreConditions);
    }

    private void ParametersSafe (
        MethodCall methodCall, out List<IPreCondition> requireSafenessParameters, out List<ProblemMetadata> parameterProblems)
    {
      ArgumentUtility.CheckNotNull ("methodCall", methodCall);

      requireSafenessParameters = new List<IPreCondition>();
      parameterProblems = new List<ProblemMetadata>();
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      string[] parameterFragmentTypes = _symbolTable.InferParameterFragmentTypes (calleeMethod);

      for (int i = 0; i < parameterFragmentTypes.Length; i++)
      {
        Expression operand = methodCall.Operands[i];
        string operandFragmentType = _symbolTable.InferFragmentType (operand);
        string parameterFragmentType = parameterFragmentTypes[i];

        if (operandFragmentType != SymbolTable.LITERAL
            && parameterFragmentType != SymbolTable.EMPTY_FRAGMENT
            && operandFragmentType != parameterFragmentType)
        {
          string variableName;
          ProblemMetadata problemMetadata = new ProblemMetadata (operand.UniqueKey, operand.SourceContext, parameterFragmentType, operandFragmentType);
          if (IntrospectionUtility.IsVariable (operand, out variableName)
              && !_symbolTable.Contains (variableName))
          {
            requireSafenessParameters.Add (new AssignabilityPreCondition (variableName, parameterFragmentType, problemMetadata));
          }
          else
          {
            parameterProblems.Add (problemMetadata);
          }
        }
      }
    }

    private void UpdateOutAndRefSymbols (MethodCall methodCall)
    {
      Method method = IntrospectionUtility.ExtractMethod (methodCall);
      for (int i = 0; i < methodCall.Operands.Count; i++)
      {
        if (IntrospectionUtility.IsVariable (methodCall.Operands[i])
            && (method.Parameters[i].IsOut || method.Parameters[i].Type is Reference))
        {
          string symbol = IntrospectionUtility.GetVariableName (methodCall.Operands[i]);
          if (FragmentUtility.ContainsFragment (method.Parameters[i].Attributes))
          {
            string fragmentType = FragmentUtility.GetFragmentType (method.Parameters[i].Attributes);
            _symbolTable.MakeSafe (symbol, fragmentType);
          }
          else
          {
            _symbolTable.MakeUnsafe (symbol);
          }
        }
      }
    }
  }
}
