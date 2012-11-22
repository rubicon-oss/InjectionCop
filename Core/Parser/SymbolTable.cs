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
using System.Linq;
using InjectionCop.Config;
using InjectionCop.Parser._Block;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  /// <summary>
  /// Keeps state (safe/unsafe) of a set of symbols and defines methods to infer state.
  /// </summary>
  public class SymbolTable : ICloneable
  {
    private readonly IBlackTypes _blackTypes;
    private Dictionary<string, Dictionary<string,bool>> _safenessMap;
    
    public SymbolTable(IBlackTypes blackTypes)
    {
      _blackTypes = blackTypes;
      _safenessMap = new Dictionary<string, Dictionary<string,bool>>();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public bool ReturnsFragment (Expression expression, out string fragmentType)
    {
      bool returnsFragment = false;
      fragmentType = string.Empty;
      if (expression is Literal)
      {
        fragmentType = "Literal";
        returnsFragment = true;
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        returnsFragment = Lookup (local.Name.Name, out fragmentType);   
      }
      else if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        returnsFragment = Lookup (parameter.Name.Name, out fragmentType);
      }
      else if (expression is MethodCall)
      {
        Method calleeMethod = IntrospectionTools.ExtractMethod((MethodCall) expression);
        if (calleeMethod.ReturnAttributes != null)
        {
          returnsFragment = FragmentTools.ContainsFragment (calleeMethod.ReturnAttributes);
          fragmentType = FragmentTools.GetFragmentType (calleeMethod.ReturnAttributes);
        }
      }
      else if (expression is UnaryExpression)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expression;
        returnsFragment = ReturnsFragment (unaryExpression.Operand, out fragmentType);
      }

      return returnsFragment;
    }

    private bool Lookup (string name, out string fragmentType)
    {
      bool returnsFragment = false;
      fragmentType = string.Empty;
      if (_safenessMap.ContainsKey (name))
      {
        foreach (string context in _safenessMap[name].Keys)
        {
          if (_safenessMap[name][context])
          {
            returnsFragment = true;
            fragmentType = context;
          }
        }
      }
      return returnsFragment;
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
          string fragmentType;
          bool isExpressionSafe = ReturnsFragment(expression, out fragmentType);
          //if (IsNotSafe (expression,"SqlFragment") && IsNotSafe(expression, "Literal"))
          if(!isExpressionSafe
            || (fragmentType != "SqlFragment" && fragmentType != "Literal"))
          {
            parameterSafe = false;
            if(IntrospectionTools.IsVariable(expression))
            {
              string variableName = IntrospectionTools.GetVariableName (expression);
              requireSafenessParameters.Add (new PreCondition(variableName, "SqlFragment"));
            }
          }
        }
      }

      for (int i = 0; i < calleeMethod.Parameters.Count; i++)
      {
        Expression expression = methodCall.Operands[i];

        bool requiresFragmentParameter = FragmentTools.ContainsFragment(calleeMethod.Parameters[i].Attributes);
        if (requiresFragmentParameter)
        {
          string fragmentParameterType = FragmentTools.GetFragmentType(calleeMethod.Parameters[i].Attributes);
          string operandParameterType;
          bool isFragmentOperand = ReturnsFragment (expression, out operandParameterType);
          if(!isFragmentOperand 
            || (fragmentParameterType != operandParameterType && operandParameterType != "Literal"))
          {
            if (IntrospectionTools.IsVariable(expression))
            {
              string variableName = IntrospectionTools.GetVariableName (expression);
              if (!_safenessMap.ContainsKey(variableName))
              {
                requireSafenessParameters.Add(new PreCondition(variableName, fragmentParameterType));
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
 
    public void InferSafeness(string symbolName, Expression expression)
    {
      if (!_safenessMap.ContainsKey (symbolName))
      {
        _safenessMap[symbolName] = new Dictionary<string, bool>();
      }
      string fragmentType;
      if (ReturnsFragment (expression, out fragmentType))
      {
        MakeUnsafe (symbolName);  
        _safenessMap[symbolName][fragmentType] = true;
      }
      else
      {
        MakeUnsafe (symbolName);
      }
    }

    public void MakeUnsafe (string symbolName)
    {
      InitializeOnce (symbolName);
      foreach (string context in _safenessMap[symbolName].Keys.ToList())
      {
        _safenessMap[symbolName][context] = false;
      }
    }

    public void MakeSafe (string symbolName, string fragmentType)
    {
      MakeUnsafe (symbolName);
      _safenessMap[symbolName][fragmentType] = true;
    }

    private void InitializeOnce (string symbolName)
    {
      if (!_safenessMap.ContainsKey (symbolName))
      {
        _safenessMap[symbolName] = new Dictionary<string, bool>();
      }
    }
    
    public Dictionary<string, bool> GetContextMap (string symbolName)
    {
      if (_safenessMap.ContainsKey (symbolName))
      {
        return new Dictionary<string, bool>(_safenessMap[symbolName]);
      }
      else
      {
        throw new InjectionCopException("Given Symbolname not found in Symboltable");
      }
    }

    public void SetContextMap (string symbol, Dictionary<string, bool> safenessMap)
    {
      _safenessMap[symbol] = safenessMap;
    }

    public bool IsSafe (string symbolName, string fragmentType)
    {
      bool isFragment = false;
      bool isLiteral = false;
      if(_safenessMap.ContainsKey(symbolName))
      {
        if(_safenessMap[symbolName].ContainsKey(fragmentType))
        {
          isFragment = _safenessMap[symbolName][fragmentType];
        }
        if(_safenessMap[symbolName].ContainsKey("Literal"))
        {
          isLiteral = _safenessMap[symbolName]["Literal"];
        }
      }
      return isFragment || isLiteral;
    }
   
    public bool Contains (string symbolName)
    {
      return _safenessMap.ContainsKey (symbolName);
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
  }
}
