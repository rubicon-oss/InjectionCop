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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class SymbolTable : ISymbolTable
  {
    public static readonly string LITERAL = "__Literal__";
    public static readonly string EMPTY_FRAGMENT = "__EmptyFragment__";

    private readonly IBlacklistManager _blacklistManager;

    private Dictionary<string, string> _safenessMap;
    
    public SymbolTable (IBlacklistManager blacklistManager)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull("blacklistManager", blacklistManager);
      _safenessMap = new Dictionary<string, string>();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public string InferFragmentType (Expression expression)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

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
      else if (expression is MemberBinding)
      {
        MemberBinding memberBinding = (MemberBinding) expression;
        fragmentType = FragmentUtility.GetMemberBindingFragmentType(memberBinding);
      }
      else if (expression is MethodCall)
      {
        Method calleeMethod = IntrospectionUtility.ExtractMethod ((MethodCall) expression);
        fragmentType = FragmentUtility.InferReturnFragmentType (calleeMethod);
      }
      else if (expression is UnaryExpression)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expression;
        fragmentType = InferFragmentType (unaryExpression.Operand);
      }
      else if (expression is Indexer)
      {
        Indexer indexer = (Indexer) expression;
        fragmentType = InferFragmentType (indexer.Object);
      }

      return fragmentType;
    }

    public bool IsAssignableTo(string symbolName, string fragmentType)
    {
      ArgumentUtility.CheckNotNull("symbolName", symbolName);
      ArgumentUtility.CheckNotNull("fragmentType", fragmentType);

      bool fragmentsMatch = false;
      bool sourceIsLiteral = false;
      bool assignmentOnEmptyFragment = fragmentType == EMPTY_FRAGMENT;
      if (_safenessMap.ContainsKey(symbolName))
      {
        fragmentsMatch = _safenessMap[symbolName] == fragmentType;
        sourceIsLiteral = _safenessMap[symbolName] == LITERAL;
      }
      return fragmentsMatch || sourceIsLiteral || assignmentOnEmptyFragment;
    }

    public string[] InferParameterFragmentTypes (Method method)
    {
      string[] parameterFragmentTypes;
      Method[] interfaceDeclarations = IntrospectionUtility.InterfaceDeclarations (method);
      
      if (interfaceDeclarations.Any())
      {
        parameterFragmentTypes = GetParameterFragmentTypes (interfaceDeclarations.First());
      }
      else
      {
        parameterFragmentTypes = GetParameterFragmentTypes (method);
      }
      
      return parameterFragmentTypes;
    }
    
    public void InferSafeness (string symbolName, Expression expression)
    {
      if (symbolName != null)
      {
        ArgumentUtility.CheckNotNull("expression", expression);
        string fragmentType = InferFragmentType(expression);
        _safenessMap[symbolName] = fragmentType;
      }
    }

    public string GetFragmentType (string symbolName)
    {
      string fragmentType = EMPTY_FRAGMENT;
      if (symbolName != null)
      {
        if (_safenessMap.ContainsKey (symbolName))
        {
          fragmentType = _safenessMap[symbolName];
        }
      }
      return fragmentType;
    }

    public void MakeUnsafe (string symbolName)
    {
      ArgumentUtility.CheckNotNull ("symbolName", symbolName);
      _safenessMap[symbolName] = EMPTY_FRAGMENT;
    }

    public void MakeSafe (string symbolName, string fragmentType)
    {
      ArgumentUtility.CheckNotNull ("symbolName", symbolName);
      ArgumentUtility.CheckNotNull ("fragmentType", fragmentType);
      _safenessMap[symbolName] = fragmentType;
    }
    
    public bool Contains (string symbolName)
    {
      ArgumentUtility.CheckNotNull ("symbolName", symbolName);
      return _safenessMap.ContainsKey (symbolName);
    }

    public ISymbolTable Copy ()
    {
      SymbolTable clone = new SymbolTable (_blacklistManager);
      clone._safenessMap = new Dictionary<string, string> (_safenessMap);
      return clone;
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
    
    private string[] GetParameterFragmentTypes(Method calleeMethod)
    {
      List<string> parameterTypes = IntrospectionUtility.GetParameterTypes(calleeMethod);
      string assemblyName = calleeMethod.ContainingAssembly().Name;
      
      FragmentSignature signature = _blacklistManager.GetFragmentTypes(assemblyName, calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name, parameterTypes);
      string[] parameterFragmentTypes;
      if (signature == null)
      {
        parameterFragmentTypes = FragmentUtility.GetAnnotatedParameterFragmentTypes(calleeMethod);
      }
      else
      {
        parameterFragmentTypes = signature.ParameterFragmentTypes;
      }
      return parameterFragmentTypes;
    }
  }
}
