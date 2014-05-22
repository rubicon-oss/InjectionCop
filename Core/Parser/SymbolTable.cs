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
using System.Linq;
using InjectionCop.Config;
using InjectionCop.Parser.CustomInferenceRules;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  public class SymbolTable : ISymbolTable
  {
    private readonly IBlacklistManager _blacklistManager;
    private Dictionary<string, Fragment> _safenessMap;
    private readonly CustomInferenceController _customInferenceController;

    public SymbolTable (IBlacklistManager blacklistManager)
    {
      _blacklistManager = ArgumentUtility.CheckNotNull("blacklistManager", blacklistManager);
      _safenessMap = new Dictionary<string, Fragment>();
      _customInferenceController = new CustomInferenceController();
    }

    public IEnumerable<string> Symbols
    {
      get { return new List<string> (_safenessMap.Keys).ToArray(); }
    }

    public Fragment InferFragmentType (Expression expression)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

      var fragmentType = Fragment.CreateEmpty();
      if (expression is Literal)
      {
        fragmentType = Fragment.CreateLiteral();
      }
      else if (expression is Local)
      {
        Local local = (Local)expression;
        fragmentType = Lookup(local.Name.Name);
      }
      else if (expression is Parameter)
      {
        Parameter parameter = (Parameter)expression;
        fragmentType = Lookup(parameter.Name.Name);
      }
      else if (expression is MemberBinding)
      {
        MemberBinding memberBinding = (MemberBinding)expression;
        fragmentType = FragmentUtility.GetMemberBindingFragmentType(memberBinding);
      }
      else if (expression is MethodCall)
      {
        MethodCall methodCall = (MethodCall)expression;
        fragmentType = InferMethodCallReturnFragmentType(methodCall);
      }
      else if (expression is UnaryExpression)
      {
        UnaryExpression unaryExpression = (UnaryExpression)expression;
        fragmentType = InferFragmentType(unaryExpression.Operand);
      }
      else if (expression is Indexer)
      {
        Indexer indexer = (Indexer)expression;
        fragmentType = InferFragmentType(indexer.Object);
      }
     
      return fragmentType;
    }

    private Fragment InferMethodCallReturnFragmentType (MethodCall methodCall)
    {
      Fragment returnFragment;
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);

      if(_customInferenceController.Infers(calleeMethod))
      {
        returnFragment = _customInferenceController.InferFragmentType (methodCall, this);
      }
      else
      {
        returnFragment = FragmentUtility.InferReturnFragmentType (calleeMethod);
      }

      return returnFragment;
    }

    public bool IsAssignableTo(string symbolName, Fragment fragmentType)
    {
      ArgumentUtility.CheckNotNull("symbolName", symbolName);
      ArgumentUtility.CheckNotNull ("fragmentType", fragmentType);

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
      var fragmentType = Fragment.CreateEmpty();
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
      _safenessMap[symbolName] = Fragment.CreateEmpty();
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
      var fragmentType = Fragment.CreateEmpty();
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
        parameterFragmentTypes = signature.ParameterFragmentTypes.Select(name => name != null ? Fragment.CreateNamed(name) : Fragment.CreateEmpty()).ToArray();
      }
      return parameterFragmentTypes;
    }
  }
}
