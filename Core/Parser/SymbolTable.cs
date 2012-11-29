﻿// Copyright 2012 rubicon informationstechnologie gmbh
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
  public class SymbolTable : ISymbolTable
  {
    private static readonly string LITERAL = "__Literal__";
    private static readonly string _emptyFragment = "__EmptyFragment__";

    private readonly IBlacklistManager _blacklistManager;

    private Dictionary<string, string> _safenessMap;
    
    public static string EMPTY_FRAGMENT 
    {
      get { return _emptyFragment; }
    }

    /*
    public SymbolTable (IBlackTypes blackTypes)
    {
      _blackTypes = blackTypes;
      _safenessMap = new Dictionary<string, string>();
    }*/

    public SymbolTable (IBlacklistManager blacklistManager)
    {
      _blacklistManager = blacklistManager;
      _safenessMap = new Dictionary<string, string>();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public string InferFragmentType (Expression expression)
    {
      string fragmentType = EMPTY_FRAGMENT;
      if (expression is Literal)
      {
        fragmentType = LITERAL;
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
      string fragmentType = EMPTY_FRAGMENT;
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
      List<string> parameterFragmentTypes = GetParameterFragmentTypes (calleeMethod);
      
      for (int i = 0; i < parameterFragmentTypes.Count; i++)
      {
        Expression operand = methodCall.Operands[i];
        string operandFragmentType = InferFragmentType (operand);
        string parameterFragmentType = parameterFragmentTypes[i];
        
        if (operandFragmentType != LITERAL
            && parameterFragmentType != EMPTY_FRAGMENT
            && operandFragmentType != parameterFragmentType)
        {
          string variableName;
          if (IntrospectionTools.IsVariable (operand, out variableName)
              && !_safenessMap.ContainsKey (variableName))
          {
            requireSafenessParameters.Add (new PreCondition (variableName, parameterFragmentType));
          }
          else
          {
            parameterSafe = false;
          }
        }
      }
      return parameterSafe;
    }

    private List<string> GetParameterFragmentTypes (Method calleeMethod)
    {
      List<string> parameterTypes = GetParameterTypes (calleeMethod);
      List<string> parameterFragmentTypes = new List<string>();
      
      if (_blacklistManager != null && _blacklistManager.IsListed (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name, parameterTypes))
      {
        parameterFragmentTypes = _blacklistManager.GetFragmentTypes (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name, parameterTypes);
      }
      else
      {
        parameterFragmentTypes.AddRange (calleeMethod.Parameters.Select (parameter => FragmentTools.GetFragmentType (parameter.Attributes)));
      }

      return parameterFragmentTypes;
    }

    private List<string> GetParameterTypes (Method method)
    {
      return method.Parameters.Select (parameter => parameter.Type.FullName).ToList();
    }

    public void InferSafeness (string symbolName, Expression expression)
    {
      string fragmentType = InferFragmentType (expression);
      _safenessMap[symbolName] = fragmentType;
    }

    public void MakeUnsafe (string symbolName)
    {
      _safenessMap[symbolName] = EMPTY_FRAGMENT;
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
        return EMPTY_FRAGMENT; //string.Empty;
      }
    }

    public bool IsFragment (string symbolName, string fragmentType)
    {
      bool isFragment = false;
      bool isLiteral = false;
      if (_safenessMap.ContainsKey (symbolName))
      {
        isFragment = _safenessMap[symbolName] == fragmentType;
        isLiteral = _safenessMap[symbolName] == LITERAL;
      }
      return isFragment || isLiteral;
    }

    public bool Contains (string symbolName)
    {
      return _safenessMap.ContainsKey (symbolName);
    }

    public ISymbolTable Copy ()
    {
      SymbolTable clone = new SymbolTable (_blacklistManager);
      clone._safenessMap = new Dictionary<string, string> (_safenessMap);
      return clone;
    }
  }
}
