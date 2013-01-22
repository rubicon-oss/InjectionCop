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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class SymbolTable : ISymbolTable
  {
    [Obsolete]
    public static readonly Fragment LITERAL = Fragment.CreateLiteral();
    [Obsolete]
    public static readonly Fragment EMPTY_FRAGMENT = null;

    private readonly IBlacklistManager _blacklistManager;

    private Dictionary<string, Fragment> _safenessMap;
    
    public SymbolTable (IBlacklistManager blacklistManager)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull("blacklistManager", blacklistManager);
      _safenessMap = new Dictionary<string, Fragment>();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public Fragment InferFragmentType (Expression expression)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

      Fragment fragmentType = EMPTY_FRAGMENT;
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

    public bool IsAssignableTo(string symbolName, Fragment fragmentType)
    {
      ArgumentUtility.CheckNotNull("symbolName", symbolName);
      ArgumentUtility.CheckNotNull("fragmentType", fragmentType);

      return FragmentUtility.FragmentTypesAssignable (GetFragmentType(symbolName), fragmentType);
    }

    public Fragment[] InferParameterFragmentTypes (Method method)
    {
      Fragment[] parameterFragmentTypes;
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
        Fragment fragmentType = InferFragmentType(expression);
        _safenessMap[symbolName] = fragmentType;
      }
    }

    public Fragment GetFragmentType (string symbolName)
    {
      Fragment fragmentType = EMPTY_FRAGMENT;
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

    public void MakeSafe (string symbolName, Fragment fragmentType)
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
      clone._safenessMap = new Dictionary<string, Fragment> (_safenessMap);
      return clone;
    }

    private Fragment Lookup (string name)
    {
      Fragment fragmentType = EMPTY_FRAGMENT;
      if (_safenessMap.ContainsKey (name))
      {
        fragmentType = _safenessMap[name];
      }
      return fragmentType;
    }
    
    private Fragment[] GetParameterFragmentTypes(Method calleeMethod)
    {
      List<string> parameterTypes = IntrospectionUtility.GetParameterTypes(calleeMethod);
      string assemblyName = calleeMethod.ContainingAssembly().Name;
      
      FragmentSignature signature = _blacklistManager.GetFragmentTypes(assemblyName, calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name, parameterTypes);
      Fragment[] parameterFragmentTypes;
      if (signature == null)
      {
        parameterFragmentTypes = FragmentUtility.GetAnnotatedParameterFragmentTypes(calleeMethod);
      }
      else
      {
        parameterFragmentTypes = signature.ParameterFragmentTypes.Select(name => name != null ? Fragment.CreateNamed(name) : null).ToArray();
      }
      return parameterFragmentTypes;
    }
  }
}
