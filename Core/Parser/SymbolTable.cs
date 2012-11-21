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
      bool isLiteral = expression is Literal;
        
      bool isFragment = FragmentTools.Returns(new FragmentAttribute(fragmentType), expression);
      bool isSafeVariable = false;
      bool isSafeUnaryExpression = false;

      if (expression is Parameter)
      {
        Parameter parameter = (Parameter) expression;
        if (_safenessMap.ContainsKey (parameter.Name.Name) && _safenessMap[parameter.Name.Name].ContainsKey(fragmentType))
        {
          isSafeVariable = _safenessMap[parameter.Name.Name][fragmentType];
        }
      }
      else if (expression is Local)
      {
        Local local = (Local) expression;
        if (_safenessMap.ContainsKey(local.Name.Name) && _safenessMap[local.Name.Name].ContainsKey(fragmentType))
        {
          isSafeVariable = _safenessMap[local.Name.Name][fragmentType];
        }
      }
      else if (expression is UnaryExpression)
      {
        isSafeUnaryExpression = IsSafe (((UnaryExpression) expression).Operand, fragmentType);
      }

      return isLiteral || isFragment || isSafeVariable || isSafeUnaryExpression;
    }

    public bool IsNotSafe (Expression expression, string fragmentType)
    {
      return !IsSafe (expression, fragmentType);
    }

    public bool ReturnsFragment (Expression expression, out string fragmentType)
    {
      bool returnsFragment = false;
      fragmentType = "";
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
        else
        {
          fragmentType = string.Empty;
        }
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
          if (_safenessMap[name][context] == true)
          {
            returnsFragment = true;
            fragmentType = context;
          }
        }
      }
      return returnsFragment;
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
          if (IsNotSafe (expression,"SqlFragment") && IsNotSafe(expression, "Literal"))
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

    public void MakeUnsafe (Identifier symbolName)
    {
      MakeUnsafe(symbolName.Name);
    }

    public void MakeUnsafe (string symbolName)
    {
      foreach (string context in _safenessMap[symbolName].Keys.ToList())
      {
        _safenessMap[symbolName][context] = false;
      }
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

    public void SetSafeness(Identifier symbolName, string fragmentType, bool value)
    {
      SetSafeness(symbolName.Name, fragmentType, value);
    }

    public void InferSafeness(Identifier symbolName, Expression expression)
    {
      // FragmentTools.ReturnsFragment(expression) check if expression returns fragment
      // -> issafe mit entsprechendem fragmenttype
      // bei literal -> any context
      // bei parameter und local in tabelle nachsehen
      if (!_safenessMap.ContainsKey (symbolName.Name))
      {
        _safenessMap[symbolName.Name] = new Dictionary<string, bool>();
      }
      string fragmentType;
      if (ReturnsFragment (expression, out fragmentType))
      {
        MakeUnsafe (symbolName);  
        _safenessMap[symbolName.Name][fragmentType] = true;
      }
      else
      {
        MakeUnsafe (symbolName);
      }
    }

    public void SetSafeness (string symbolName, string fragmentType, bool safeness)
    {
      if (_safenessMap.ContainsKey(symbolName))
      {
        if (safeness == true)
        {
          MakeUnsafe (symbolName);
        }
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
