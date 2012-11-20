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
    private Dictionary<string, Dictionary<string,bool>> _safenessMap;
    //private readonly FragmentAttribute sqlFragment = new FragmentAttribute ("SqlFragment");

    public SymbolTable(IBlackTypes blackTypes)
    {
      _blackTypes = blackTypes;
      _safenessMap = new Dictionary<string, Dictionary<string,bool>>();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public bool IsSafe (Expression expression, string fragmentType)
    {
      bool literalOrFragment = expression is Literal || FragmentTools.Returns(new FragmentAttribute(fragmentType), expression);
      bool safeVariable = false;
      bool safeUnaryExpression = false;

      if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        if (_safenessMap.ContainsKey (parameter.Name.Name) && _safenessMap[parameter.Name.Name].ContainsKey(fragmentType))
        {
          safeVariable = _safenessMap[parameter.Name.Name][fragmentType];
        }
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        if (_safenessMap.ContainsKey(local.Name.Name) && _safenessMap[local.Name.Name].ContainsKey(fragmentType))
        {
          safeVariable = _safenessMap[local.Name.Name][fragmentType];
        }
      }
      else if (expression is UnaryExpression)
      {
        safeUnaryExpression = IsSafe (((UnaryExpression) expression).Operand, fragmentType);
      }

      return literalOrFragment || safeVariable || safeUnaryExpression;
    }

    public bool IsNotSafe (Expression expression, string fragmentType)
    {
      return !IsSafe (expression, fragmentType);
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
          if (IsNotSafe (expression,"SqlFragment"))
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
        //bool isFragmentParameter = FragmentTools.Contains (sqlFragment, calleeMethod.Parameters[i].Attributes);
        bool isFragmentParameter = FragmentTools.ContainsFragment(calleeMethod.Parameters[i].Attributes);
        
        Expression expression = methodCall.Operands[i];
        
        /*if (isFragmentParameter)
        {
          if (IsVariable (expression))
          {
            requireSafenessParameters.Add (VariableName (expression));
          }
          if (IsNotSafe (expression))
          {
            parameterSafe = false;
          }

        }*/

        if (isFragmentParameter)
        {
          string fragmentType = FragmentTools.GetFragmentType(calleeMethod.Parameters[i].Attributes);
          if (IsNotSafe(expression, fragmentType))
          {
            if (IsVariable(expression))
            /*&& _safenessMap[VariableName(expression)] == false*/
            {
              if (!_safenessMap.ContainsKey(VariableName(expression)))
              {
                requireSafenessParameters.Add(VariableName(expression));
              }
              else
              {
                parameterSafe = false;
              }
            }
            else
            {
              parameterSafe = false;
            }
          }
        }
        
      }

      return parameterSafe;
    }

    private bool IsVariable(Expression expression)
    {
      bool isVariableReference = false;
      if (expression.NodeType == NodeType.AddressOf)
      {
        Local operand = ((UnaryExpression) expression).Operand as Local;
        if (operand != null)
        {
          isVariableReference = IsVariable (operand);
        }
      }
      return expression is Parameter || expression is Local || isVariableReference;
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
      else if (expression.NodeType == NodeType.AddressOf)
      {
        Local operand = ((UnaryExpression) expression).Operand as Local;
        if (operand != null)
        {
          variableName = operand.Name.Name;
        }
      }
      return variableName;
    }

    public void SetSafeness(Identifier symbolName, string fragmentType, bool value)
    {
      SetSafeness(symbolName.Name, fragmentType, value);
    }

    public void SetSafeness(Identifier symbolName, string fragmentType, Expression expression)
    {
      SetSafeness (symbolName, fragmentType, IsSafe (expression, fragmentType));
    }

    public SymbolTable Clone()
    {
      SymbolTable clone = new SymbolTable (_blackTypes);
      clone._safenessMap = new Dictionary<string, Dictionary<string,bool>>();
      foreach (string symbol in _safenessMap.Keys)
      {
        clone._safenessMap[symbol] = new Dictionary<string, bool>(_safenessMap[symbol]);
      }
      return clone;
    }

    object ICloneable.Clone()
    {
      return Clone();
    }

    public void SetSafeness (string symbolName, string fragmentType, bool safeness)
    {
      if (_safenessMap.ContainsKey(symbolName))
      {
        _safenessMap[symbolName][fragmentType] = safeness;
      }
      else
      {
        _safenessMap[symbolName] = new Dictionary<string, bool>();
        _safenessMap[symbolName][fragmentType] = safeness;
      }
    }

    public bool IsSafe (string symbolName, string fragmentType)
    {
      bool isSafe = false;
      if(_safenessMap.ContainsKey(symbolName) && _safenessMap[symbolName].ContainsKey(fragmentType))
      {
        isSafe = _safenessMap[symbolName][fragmentType];
      }
      return isSafe;
    }

    public bool IsNotSafe (string symbolName, string fragmentType)
    {
      return !IsSafe (symbolName, fragmentType);
    }

    public bool Contains (string symbolName)
    {
      return _safenessMap.ContainsKey (symbolName);
    }
  }
}
