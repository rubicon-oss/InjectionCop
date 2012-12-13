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
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Utilities
{
  /// <summary>
  /// Helper methods for dealing with FxCop
  /// </summary>
  public class IntrospectionUtility
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
      
      return expression is Parameter
             || expression is Local
             || IsVariableReference(expression)
             || IsField(expression);
    }

    private static bool IsVariableReference (Expression expression)
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
      return isVariableReference;
    }

    public static bool IsField (Expression expression)
    {
      bool isField = false;
      if (expression is MemberBinding)
      {
        MemberBinding memberBinding = (MemberBinding) expression;
        isField = memberBinding.BoundMember is Field;
      }
      return isField;
    }

    public static Field GetField (Expression expression)
    {
      Field field = null;
      if (IsField (expression))
      {
        MemberBinding memberBinding = (MemberBinding) expression;
        field = (Field) memberBinding.BoundMember;
      }
      return field;
    }

    public static string GetVariableName (Expression expression)
    {
      ArgumentUtility.CheckNotNull ("expression", expression);

      string variableName = null;
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
      else if (expression is MemberBinding)
      {
        MemberBinding memberBinding = (MemberBinding) expression;
        if (memberBinding.BoundMember is Field)
        {
          Field field = (Field) memberBinding.BoundMember;
          variableName = field.Name.Name;
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
      return TypeNodeFactory (targetType);
    }

    public static Method MethodFactory<T> (string methodName, params TypeNode[] methodParameters)
    {
      Type targetTypeNode = typeof(T);
      return MethodFactory(targetTypeNode, methodName, methodParameters);
    }

    public static Method MethodFactory (Type targetType, string methodName, params TypeNode[] methodParameters)
    {
      ArgumentUtility.CheckNotNull ("methodName", methodName);
      ArgumentUtility.CheckNotNull ("methodParameters", methodParameters);

      TypeNode targetTypeNode = TypeNodeFactory(targetType);
      Identifier methodIdentifier = Identifier.For (methodName);
      Method targetMethod = targetTypeNode.GetMethod (methodIdentifier, methodParameters);
      return targetMethod;
    }

    public static TypeNode TypeNodeFactory (Type targetType)
    {
      ArgumentUtility.CheckNotNull ("targetType", targetType);
      string targetLocation = targetType.Assembly.Location;
      AssemblyNode targetAssembly = AssemblyNode.GetAssembly (targetLocation);
      Identifier targetNamespace = Identifier.For (targetType.Namespace);
      Identifier targetName = Identifier.For (targetType.Name);
      return targetAssembly.GetType (targetNamespace, targetName);
    }

    /// <summary>
    /// Property getters are transformed to methdods named get_PROPERTYNAME()
    /// </summary>
    public static bool IsPropertyGetter (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      bool isPropertyGetter = false;
      string[] nameParts = method.Name.Name.Split ('_');
      if (method.DeclaringMember != null
          && method.DeclaringMember.NodeType == NodeType.Property
          && nameParts.Length >= 2
          && "get" == nameParts[0])
      {
        isPropertyGetter = true;
      }
      return isPropertyGetter;
    }

    public static bool IsPropertySetter (Method method)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      bool isPropertySetter = false;
      string[] nameParts = method.Name.Name.Split ('_');
      if (method.DeclaringMember != null
          && method.DeclaringMember.NodeType == NodeType.Property
          && nameParts.Length >= 2
          && "set" == nameParts[0])
      {
        isPropertySetter = true;
      }
      return isPropertySetter;
    }
  }
}
