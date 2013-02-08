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
using System.Linq;
using System.Collections.Generic;
using InjectionCop.Parser.BlockParsing.PreCondition;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.CustomInferenceRules
{
  public class StringBuilderInference: ICustomInference
  {
    private readonly string[] _safeMethods;
    private readonly string[] _unsafeMethods;
    private readonly string[] _fragmentParameterInferenceMethods;
    private readonly string[] _fragmentInferringMethods;

    public StringBuilderInference ()
    {
      _safeMethods = new[]
                     {
                         "System.Text.StringBuilder.Append(System.Boolean)",
                         "System.Text.StringBuilder.Append(System.Byte)",
                         "System.Text.StringBuilder.Append(System.SByte)",
                         "System.Text.StringBuilder.Append(System.Int16)",
                         "System.Text.StringBuilder.Append(System.Int32)",
                         "System.Text.StringBuilder.Append(System.Int64)",
                         "System.Text.StringBuilder.Append(System.UInt16)",
                         "System.Text.StringBuilder.Append(System.UInt32)",
                         "System.Text.StringBuilder.Append(System.UInt64)"
                     };

      _unsafeMethods = new[]
                       {
                           "System.Text.StringBuilder.Append(System.Decimal)",
                           "System.Text.StringBuilder.Append(System.Double)",
                           "System.Text.StringBuilder.Append(System.Single)"
                       };

      _fragmentParameterInferenceMethods = new[]
                                  {
                                      "System.Text.StringBuilder.Append(System.String)",
                                      "System.Text.StringBuilder.AppendFormat(System.String,System.Object)",
                                      "System.Text.StringBuilder.AppendFormat(System.String,System.Object,System.Object)",
                                      "System.Text.StringBuilder.AppendFormat(System.String,System.Object,System.Object,System.Object)",
                                      "System.Text.StringBuilder.AppendFormat(System.String,System.Object[])"
                                  };

      _fragmentInferringMethods = new[]
                       { "System.Text.StringBuilder.ToString()" };
    }

    public bool Analyzes (Method method)
    {
      return IsSafeMethod (method) || IsUnsafeMethod (method) || IsFragmentParameterInferenceMethod (method);
    }

    public Fragment InferFragmentType(MethodCall methodCall, ISymbolTable context)
    {
      Fragment returnFragment = Fragment.CreateEmpty();
      Method method = IntrospectionUtility.ExtractMethod (methodCall);
      if (Infers (method) && methodCall.Callee is MemberBinding)
      {
        MemberBinding memberBinding = (MemberBinding) methodCall.Callee;
        string variableName;
        if (IntrospectionUtility.IsVariable (memberBinding.TargetObject, out variableName))
        {
          returnFragment = context.GetFragmentType (variableName);
        }
      }
      return returnFragment;
    }
    
    public void PassProblem (MethodCall methodCall, List<IPreCondition> preConditions, ProblemMetadata problemMetadata, ISymbolTable symbolTable, IProblemPipe problemPipe)
    {
      throw new NotImplementedException();
    }

    public void Analyze (MethodCall methodCall, ISymbolTable context, List<IPreCondition> preConditions)
    {
      Method method = IntrospectionUtility.ExtractMethod (methodCall);
      if (Analyzes(method) && methodCall.Callee is MemberBinding)
      {
        MemberBinding memberBinding = (MemberBinding) methodCall.Callee;
        
        if (IsFragmentParameterInferenceMethod (method))
        {
          string variableName;
          if (IntrospectionUtility.IsVariable (memberBinding.TargetObject, out variableName))
          {
            Fragment parameterFragment = ParameterFragmentUtility.ParameterFragmentIntersection (methodCall, context);
            Fragment targetObjectFragment = context.GetFragmentType (variableName);
            if (targetObjectFragment == Fragment.CreateLiteral() || targetObjectFragment.Undefined)
            {
              context.MakeSafe (variableName, parameterFragment);
            }
            else 
            {
              if (targetObjectFragment == Fragment.CreateEmpty() && parameterFragment != Fragment.CreateLiteral()) // && parameterFragment != Fragment.CreateEmpty()
              {
                ProblemMetadata problemMetadata = new ProblemMetadata(methodCall.UniqueKey, methodCall.SourceContext, parameterFragment, targetObjectFragment);
                IPreCondition precondition = new CustomInferencePreCondition(variableName, parameterFragment, problemMetadata);
                preConditions.Add(precondition);
              }
              else if (!FragmentUtility.FragmentTypesAssignable(parameterFragment, targetObjectFragment))
              {
                context.MakeUnsafe(variableName);
              }
            }
            

          }
        }
        else if (!IsSafeMethod(method))
        {
          string variableName;
          if (IntrospectionUtility.IsVariable(memberBinding.TargetObject, out variableName))
          {
            context.MakeUnsafe(variableName);
          }
        }
      }
     
    }

    private bool IsSafeMethod (Method method)
    {
      return _safeMethods.Any (safeMethod => safeMethod == method.FullName);
    }

    private bool IsUnsafeMethod (Method method)
    {
      return _unsafeMethods.Any (unsafeMethod => unsafeMethod == method.FullName);
    }

    private bool IsFragmentParameterInferenceMethod (Method method)
    {
      return _fragmentParameterInferenceMethods.Any (coveredMethodFullName => coveredMethodFullName == method.FullName);
    }

    public bool Infers (Method method)
    {
      return _fragmentInferringMethods.Any (methodFullName => methodFullName == method.FullName);
    }

  }
}
