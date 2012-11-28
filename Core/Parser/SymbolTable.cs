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
using InjectionCop.Config;
using InjectionCop.Parser._Block;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  /// <summary>
  /// Keeps state (safe/unsafe) of a set of symbols and defines methods to infer state.
  /// </summary>
  public class SymbolTable : ISymbolTable
  {
    private static readonly string _literal = "__Literal__";
    private static readonly string _emptyFragment = "__EmptyFragment__";

    private readonly IBlackTypes _blackTypes;
    private Dictionary<string, string> _safenessMap;

    
    private static string Literal 
    {
      get { return _literal; }
    }

    public static string EmptyFragment 
    {
      get { return _emptyFragment; }
    }

    public SymbolTable (IBlackTypes blackTypes)
    {
      _blackTypes = blackTypes;
      _safenessMap = new Dictionary<string, string>();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public string InferFragmentType (Expression expression)
    {
      string fragmentType = EmptyFragment;  //string.Empty;
      if (expression is Literal)
      {
        fragmentType = Literal;
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        fragmentType = Lookup (local.Name.Name);
      }
      else if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        fragmentType = Lookup (parameter.Name.Name);
      }
      else if (expression is MethodCall)
      {
        Method calleeMethod = IntrospectionTools.ExtractMethod ((MethodCall) expression);
        if (calleeMethod.ReturnAttributes != null)
        {
          fragmentType = FragmentTools.GetFragmentType (calleeMethod.ReturnAttributes);
        }
      }
      else if (expression is UnaryExpression)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expression;
        fragmentType = InferFragmentType (unaryExpression.Operand);
      }

      return fragmentType;
    }

    private string Lookup (string name)
    {
      string fragmentType = EmptyFragment; //string.Empty;
      if (_safenessMap.ContainsKey (name))
      {
        fragmentType = _safenessMap[name];
      }
      return fragmentType;
    }

    public bool ParametersSafe (MethodCall methodCall, out List<PreCondition> requireSafenessParameters)
    {
      bool parameterSafe = true;
      requireSafenessParameters = new List<PreCondition>();
      Method calleeMethod = IntrospectionTools.ExtractMethod (methodCall);


      if (_blackTypes.IsBlackMethod (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name))
      {
        foreach (Expression expression in methodCall.Operands)
        {
          string fragmentType = InferFragmentType (expression);
          bool isExpressionSafe = fragmentType != EmptyFragment;  //string.Empty;
          //if (IsNotSafe (expression,"SqlFragment") && IsNotSafe(expression, "Literal"))
          if (!isExpressionSafe
              || (fragmentType != "SqlFragment" && fragmentType != Literal))
          {
            parameterSafe = false;
            if (IntrospectionTools.IsVariable (expression))
            {
              string variableName = IntrospectionTools.GetVariableName (expression);
              requireSafenessParameters.Add (new PreCondition (variableName, "SqlFragment"));
            }
          }
        }
      }

      for (int i = 0; i < calleeMethod.Parameters.Count; i++)
      {
        Expression expression = methodCall.Operands[i];

        bool requiresFragmentParameter = FragmentTools.ContainsFragment (calleeMethod.Parameters[i].Attributes);
        if (requiresFragmentParameter)
        {
          string fragmentParameterType = FragmentTools.GetFragmentType (calleeMethod.Parameters[i].Attributes);
          string operandParameterType = InferFragmentType (expression);
          bool isFragmentOperand = operandParameterType != EmptyFragment; //string.Empty;
          if (!isFragmentOperand
              || (fragmentParameterType != operandParameterType && operandParameterType != Literal))
          {
            if (IntrospectionTools.IsVariable (expression))
            {
              string variableName = IntrospectionTools.GetVariableName (expression);
              if (!_safenessMap.ContainsKey (variableName))
              {
                requireSafenessParameters.Add (new PreCondition (variableName, fragmentParameterType));
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
          else
          {
            parameterSafe = fragmentParameterType == operandParameterType
                            || operandParameterType == Literal;
          }
        }
      }

      return parameterSafe;
    }

    public void InferSafeness (string symbolName, Expression expression)
    {
      string fragmentType = InferFragmentType (expression);
      _safenessMap[symbolName] = fragmentType;
    }

    public void MakeUnsafe (string symbolName)
    {
      _safenessMap[symbolName] = EmptyFragment; //string.Empty;
    }

    public void MakeSafe (string symbolName, string fragmentType)
    {
      _safenessMap[symbolName] = fragmentType;
    }

    public string GetFragmentType (string symbolName)
    {
      if (_safenessMap.ContainsKey (symbolName))
      {
        return _safenessMap[symbolName];
      }
      else
      {
        return EmptyFragment; //string.Empty;
      }
    }

    public bool IsFragment (string symbolName, string fragmentType)
    {
      bool isFragment = false;
      bool isLiteral = false;
      if (_safenessMap.ContainsKey (symbolName))
      {
        isFragment = _safenessMap[symbolName] == fragmentType;
        isLiteral = _safenessMap[symbolName] == Literal;
      }
      return isFragment || isLiteral;
    }

    public bool Contains (string symbolName)
    {
      return _safenessMap.ContainsKey (symbolName);
    }

    public ISymbolTable Copy ()
    {
      SymbolTable clone = new SymbolTable (_blackTypes);
      clone._safenessMap = new Dictionary<string, string> (_safenessMap);
      return clone;
    }
  }
}
