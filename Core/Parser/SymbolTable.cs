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
using System.Collections.Generic;
using InjectionCop.Attributes;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class SymbolTable : ICloneable
  {
    private readonly IBlackTypes _blackTypes;
    private Dictionary<string, bool> _safenessMap;
    private readonly FragmentAttribute sqlFragment = new FragmentAttribute ("SqlFragment");

    public SymbolTable(IBlackTypes blackTypes)
    {
      _blackTypes = blackTypes;
      _safenessMap = new Dictionary<string, bool>();
    }

    public bool IsSafe (Expression expression)
    {
      bool literalOrFragment = expression is Literal || FragmentTools.Returns(sqlFragment, expression);
      bool safeVariable = false;
      bool safeUnaryExpression = false;

      if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        if (_safenessMap.ContainsKey (parameter.Name.Name))
        {
          safeVariable = _safenessMap[parameter.Name.Name];
        }
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        if (_safenessMap.ContainsKey (local.Name.Name))
        {
          safeVariable = _safenessMap[local.Name.Name];
        }
      }
      else if (expression is UnaryExpression)
      {
        safeUnaryExpression = IsSafe (((UnaryExpression) expression).Operand);
      }

      return literalOrFragment || safeVariable || safeUnaryExpression;
    }

    public bool IsNotSafe (Expression expression)
    {
      return !IsSafe (expression);
    }

    public bool ParametersSafe (MethodCall methodCall, out List<string> requireSafenessParameters)
    {
      bool parameterSafe = true;
      requireSafenessParameters = new List<string>();
      Method calleeMethod = IntrospectionTools.ExtractMethod (methodCall);

      if (_blackTypes.IsBlackMethod (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name))
      {
        foreach (Expression expression in methodCall.Operands)
        {
          if (IsNotSafe (expression))
          {
            parameterSafe = false;
            if(IsVariable(expression))
            {
              requireSafenessParameters.Add (VariableName (expression));
            }
          }
        }
      }

      for (int i = 0; i < calleeMethod.Parameters.Count; i++)
      {
        bool isFragmentParameter = FragmentTools.Contains (sqlFragment, calleeMethod.Parameters[i].Attributes);
        Expression expression = methodCall.Operands[i];
        if (isFragmentParameter && IsNotSafe (expression))
        {
          parameterSafe = false;
          if (IsVariable (expression))
          {
            requireSafenessParameters.Add (VariableName (expression));
          }
        }
      }

      return parameterSafe;
    }

    private bool IsVariable(Expression expression)
    {
      return expression is Parameter || expression is Local;
    }

    private string VariableName (Expression expression)
    {
      string variableName = "";
      if (expression is Parameter)
      {
        Parameter operand = (Parameter) expression;
        variableName = operand.Name.Name;
      }
      else if (expression is Local)
      {
        Local operand = (Local) expression;
        variableName = operand.Name.Name;
      }
      return variableName;
    }

    public void SetSafeness(Identifier key, bool value)
    {
      _safenessMap[key.Name] = value;
    }

    public void SetSafeness(Identifier key, Expression expression)
    {
      SetSafeness (key, IsSafe (expression));
    }

    public SymbolTable Clone()
    {
      SymbolTable clone = new SymbolTable (_blackTypes);
      clone._safenessMap = new Dictionary<string, bool> (_safenessMap);
      return clone;
    }

    object ICloneable.Clone()
    {
      return Clone();
    }

    public void SetSafeness (string key, bool safeness)
    {
      _safenessMap[key] = safeness;
    }

    public bool IsSafe (string symbolName)
    {
      bool isSafe = false;
      if(_safenessMap.ContainsKey(symbolName))
      {
        isSafe = _safenessMap[symbolName];
      }
      return isSafe;
    }

    public bool IsNotSafe (string symbolName)
    {
      return !IsSafe (symbolName);
    }
  }
}
