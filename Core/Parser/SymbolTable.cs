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
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Parser.ProblemPipe;
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

    public bool IsFragment (string symbolName, string fragmentType)
    {
      ArgumentUtility.CheckNotNull ("symbolName", symbolName);
      ArgumentUtility.CheckNotNull ("fragmentType", fragmentType);

      bool isFragment = false;
      bool isLiteral = false;
      if (_safenessMap.ContainsKey (symbolName))
      {
        isFragment = _safenessMap[symbolName] == fragmentType;
        isLiteral = _safenessMap[symbolName] == LITERAL;
      }
      return isFragment || isLiteral;
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
        if (memberBinding.BoundMember is Field)
        {
          Field field = (Field) memberBinding.BoundMember;
          fragmentType = FragmentUtility.GetFragmentType (field.Attributes);
        }
      }
      else if (expression is MethodCall)
      {
        Method calleeMethod = IntrospectionUtility.ExtractMethod ((MethodCall) expression);
        fragmentType = InferFragmentType (calleeMethod);
      }
      else if (expression is UnaryExpression)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expression;
        fragmentType = InferFragmentType (unaryExpression.Operand);
      }

      return fragmentType;
    }

    private string InferFragmentType (Method method)
    {
      string fragmentType = EMPTY_FRAGMENT;
      AttributeNodeCollection candidateAttributes;
      Method[] interfaceDeclarations = IntrospectionUtility.InterfaceDeclarations (method);
      if (interfaceDeclarations.Any())
      {
        candidateAttributes = interfaceDeclarations.First().ReturnAttributes;
      }
      else if (!IsAnnotatedPropertyGetter(method))
      {
        candidateAttributes = method.ReturnAttributes;
      }
      else
      {
        candidateAttributes = method.DeclaringMember.Attributes;
      }

      if (candidateAttributes != null)
      {
        fragmentType = FragmentUtility.GetFragmentType (candidateAttributes);
      }
      
      return fragmentType;
    }
    
    public void ParametersSafe (MethodCall methodCall, out List<PreCondition> requireSafenessParameters, out List<ProblemMetadata> parameterProblems)
    {
      ArgumentUtility.CheckNotNull ("methodCall", methodCall);
      
      requireSafenessParameters = new List<PreCondition>();
      parameterProblems = new List<ProblemMetadata>();
      Method calleeMethod = IntrospectionUtility.ExtractMethod (methodCall);
      string[] parameterFragmentTypes = InferParameterFragmentTypes (calleeMethod);
      
      for (int i = 0; i < parameterFragmentTypes.Length; i++)
      {
        Expression operand = methodCall.Operands[i];
        string operandFragmentType = InferFragmentType (operand);
        string parameterFragmentType = parameterFragmentTypes[i];
        
        if (operandFragmentType != LITERAL
            && parameterFragmentType != EMPTY_FRAGMENT
            && operandFragmentType != parameterFragmentType)
        {
          string variableName;
          ProblemMetadata problemMetadata = new ProblemMetadata (operand.UniqueKey, operand.SourceContext, parameterFragmentType, operandFragmentType);
          if (IntrospectionUtility.IsVariable (operand, out variableName)
              && !_safenessMap.ContainsKey (variableName))
          {
            requireSafenessParameters.Add (new PreCondition (variableName, parameterFragmentType, problemMetadata));
          }
          else
          {
            parameterProblems.Add (problemMetadata);
          }
        }
      }
    }

    private string[] InferParameterFragmentTypes (Method calleeMethod)
    {
      string[] parameterFragmentTypes;
      Method[] interfaceDeclarations = IntrospectionUtility.InterfaceDeclarations (calleeMethod);
      
      if (interfaceDeclarations.Any())
      {
        parameterFragmentTypes = GetParameterFragmentTypes (interfaceDeclarations.First());
      }
      else
      {
        parameterFragmentTypes = GetParameterFragmentTypes (calleeMethod);
      }
      
      return parameterFragmentTypes;
    }
    
    public void InferSafeness (string symbolName, Expression expression)
    {
      ArgumentUtility.CheckNotNullOrEmpty ("symbolName", symbolName);
      ArgumentUtility.CheckNotNull ("expression", expression);

      string fragmentType = InferFragmentType (expression);
      _safenessMap[symbolName] = fragmentType;
    }

    public string GetFragmentType (string symbolName)
    {
      ArgumentUtility.CheckNotNull ("symbolName", symbolName);

      if (_safenessMap.ContainsKey (symbolName))
      {
        return _safenessMap[symbolName];
      }
      else
      {
        return EMPTY_FRAGMENT;
      }
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

    private string[] GetParameterFragmentTypes (Method calleeMethod)
    {
      List<string> parameterTypes = GetParameterTypes (calleeMethod);
      
      string[] parameterFragmentTypes = _blacklistManager.GetFragmentTypes (calleeMethod.DeclaringType.FullName, calleeMethod.Name.Name, parameterTypes);
      if (parameterFragmentTypes == null)
      {
        List<string> buffer = new List<string>();

        if (!IsAnnotatedPropertySetter (calleeMethod))
        {
          buffer.AddRange (calleeMethod.Parameters.Select (parameter => FragmentUtility.GetFragmentType (parameter.Attributes)));
        }
        else
        {
          buffer.Add (FragmentUtility.GetFragmentType (calleeMethod.DeclaringMember.Attributes));
        }
        
        parameterFragmentTypes = buffer.ToArray();
      }

      return parameterFragmentTypes;
    }

    private bool IsAnnotatedPropertySetter (Method method)
    {
      bool isAnnotatedPropertySetter = false;
      if (IntrospectionUtility.IsPropertySetter (method))
      {
        isAnnotatedPropertySetter = FragmentUtility.ContainsFragment (method.DeclaringMember.Attributes);
      }
      return isAnnotatedPropertySetter;
    }

    private bool IsAnnotatedPropertyGetter (Method method)
    {
      bool isAnnotatedPropertyGetter = false;
      if (IntrospectionUtility.IsPropertyGetter (method))
      {
        isAnnotatedPropertyGetter = FragmentUtility.ContainsFragment (method.DeclaringMember.Attributes);
      }
      return isAnnotatedPropertyGetter;
    }

    private List<string> GetParameterTypes (Method method)
    {
      return method.Parameters.Select (parameter => parameter.Type.FullName).ToList();
    }
  }
}
