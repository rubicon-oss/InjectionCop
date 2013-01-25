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
using InjectionCop.Parser.CustomInferenceRules;
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
    private Fragment[] _parameterFragmentTypes;

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
      ArgumentUtility.CheckNotNull ("methodCall", methodCall);
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      
      _parameterFragmentTypes = _symbolTable.InferParameterFragmentTypes (calleeMethod);

      for (int i = 0; i < _parameterFragmentTypes.Length; i++)
      {
        Expression operand = methodCall.Operands[i];
        Fragment expectedFragment = _parameterFragmentTypes[i];
        CheckParameter (operand, expectedFragment);
      }
    }

    private void CheckParameter (Expression operand, Fragment expectedFragment)
    {
      Fragment operandFragmentType = _symbolTable.InferFragmentType (operand);
      
      if (!FragmentUtility.FragmentTypesAssignable (operandFragmentType, expectedFragment))
      {
        ProblemMetadata problemMetadata = new ProblemMetadata (operand.UniqueKey, operand.SourceContext, expectedFragment, operandFragmentType);
        PassProblem (operand, problemMetadata);
      }
    }

    private void PassProblem (Expression operand, ProblemMetadata problemMetadata)
    {
      string variableName;
      Fragment expectedFragment = problemMetadata.ExpectedFragment;

      if (OperandIsVariableFromPrecedingBlock (operand, out variableName))
      {
        _preConditions.Add (new AssignabilityPreCondition (variableName, expectedFragment, problemMetadata));
      }
      else if (operand is MethodCall)
      {
        MethodCall methodCall = (MethodCall) operand;
        Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);

        var binaryConcatInference = new FragmentParameterInference();
        if (binaryConcatInference.Covers(calleeMethod.FullName))
        {
          binaryConcatInference.PassProblem (methodCall, _preConditions, problemMetadata, _symbolTable, _problemPipe);
        }
        else
        {
          _problemPipe.AddProblem (problemMetadata);
        }
      }
      else
      {
        _problemPipe.AddProblem (problemMetadata);
      }
    }

    private bool OperandIsVariableFromPrecedingBlock (Expression operand, out string variableName)
    {
      return IntrospectionUtility.IsVariable (operand, out variableName)
             && !_symbolTable.Contains (variableName);
    }

    private void UpdateOutAndRefSymbols (MethodCall methodCall)
    {
      Method method = IntrospectionUtility.ExtractMethod (methodCall);
      for (int i = 0; i < methodCall.Operands.Count; i++)
      {
        bool isReturnParameter = IntrospectionUtility.IsVariable (methodCall.Operands[i])
                                 && (method.Parameters[i].IsOut || method.Parameters[i].Type is Reference);

        if (isReturnParameter)
        {
          PassReturnFragmentTypeToContext (methodCall.Operands[i], method.Parameters[i]);
        }
      }
    }

    private void PassReturnFragmentTypeToContext (Expression operand, Parameter parameter)
    {
      string symbol = IntrospectionUtility.GetVariableName (operand);
      Fragment fragmentType = FragmentUtility.GetFragmentType (parameter.Attributes);

      if (FragmentUtility.ContainsFragment (parameter.Attributes))
      {
        _symbolTable.MakeSafe (symbol, fragmentType);
      }
      else
      {
        _symbolTable.MakeUnsafe (symbol);
      }
    }
  }
}
