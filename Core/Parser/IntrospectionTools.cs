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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser
{
  /// <summary>
  /// Helper methods for dealing with FxCop
  /// </summary>
  public class IntrospectionTools
  {
    public static Method ExtractMethod (MethodCall methodCall)
    {
      ArgumentUtility.CheckNotNull ("methodCall", methodCall);

      MemberBinding callee = methodCall.Callee as MemberBinding;
      if (callee == null || !(callee.BoundMember is Method))
        throw new InjectionCopException ("Cannot extract Method from Methodcall");

      Method boundMember = (Method) callee.BoundMember;
      return boundMember;
    }

    public static bool IsVariable (Expression expression)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

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

    public static string GetVariableName (Expression expression)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

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

    public static bool IsVariable (Expression expression, out string variableName)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

      variableName = GetVariableName (expression);
      return IsVariable (expression);
    }

    public static TypeNode TypeNodeFactory<T> ()
    {
      Type targetType = typeof (T);
      string targetLocation = targetType.Assembly.Location;
      AssemblyNode targetAssembly = AssemblyNode.GetAssembly (targetLocation);
      Identifier targetNamespace = Identifier.For (targetType.Namespace);
      Identifier targetName = Identifier.For (targetType.Name);
      return targetAssembly.GetType (targetNamespace, targetName);
    }

    public static Method MethodFactory<T> (Identifier methodName, params TypeNode[] methodParameters)
    {
      ArgumentUtility.CheckNotNull ("methodName", methodName);
      ArgumentUtility.CheckNotNull ("methodParameters", methodParameters);

      TypeNode ct = TypeNodeFactory<T>();
      return ct.GetMethod (methodName, methodParameters);
    }
  }
}
