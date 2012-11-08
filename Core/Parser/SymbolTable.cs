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
  public class SymbolTable
  {
    private readonly IBlackTypes _blackTypes;
    private readonly Dictionary<Identifier, bool> _safeSymbols;
    private FragmentAttribute sqlFragment = new FragmentAttribute ("SqlFragment");

    public SymbolTable(IBlackTypes blackTypes)
    {
      _blackTypes = blackTypes;
      _safeSymbols = new Dictionary<Identifier, bool>();
    }

    public bool IsSafe (Expression expression)
    {
      bool literalOrFragment = expression is Literal || FragmentTools.Returns(sqlFragment, expression);
      bool safeVariable = false;
      bool safeUnaryExpression = false;

      if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        if (_safeSymbols.ContainsKey (parameter.Name))
        {
          safeVariable = _safeSymbols[parameter.Name];
        }
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        if (_safeSymbols.ContainsKey (local.Name))
        {
          safeVariable = _safeSymbols[local.Name];
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

    public bool ParametersSafe (MethodCall methodCall)
    {
      bool parameterSafe = true;
      Method calleeMethod = IntrospectionTools.ExtractMethod (methodCall);

      if (_blackTypes.IsBlackMethod (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name))
      {
        foreach (Expression expression in methodCall.Operands)
        {
          if (IsNotSafe (expression))
          {
            parameterSafe = false;
          }
        }
      }

      for (int i = 0; i < calleeMethod.Parameters.Count; i++)
      {
        bool isFragmentParameter = FragmentTools.Contains(sqlFragment, calleeMethod.Parameters[i].Attributes);
        if (isFragmentParameter && IsNotSafe (methodCall.Operands[i]))
        {
          parameterSafe = false;
        }
      }

      return parameterSafe;
    }

    public void SetSafeness(Identifier key, bool value)
    {
      _safeSymbols[key] = value;
    }

    public void SetSafeness(Identifier key, Expression expression)
    {
      SetSafeness (key, IsSafe (expression));
    }
  }
}
