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
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class StringBuilderConstructStatementHandler: AssignmentStatementHandlerBase
  {
    private readonly List<string> _stringBuilderConstructorFullnames = new List<string>
                                                                       {
                                                                           "System.Text.StringBuilder.#ctor",
                                                                           "System.Text.StringBuilder.#ctor(System.Int32)",
                                                                           "System.Text.StringBuilder.#ctor(System.Int32,System.Int32)",
                                                                           "System.Text.StringBuilder.#ctor(System.String)",
                                                                           "System.Text.StringBuilder.#ctor(System.String,System.Int32)",
                                                                           "System.Text.StringBuilder.#ctor(System.String,System.Int32,System.Int32,System.Int32)"
                                                                       };

    public StringBuilderConstructStatementHandler (BlockParserContext blockParserContext)
        : base(blockParserContext)
    {
    }
    
    protected override void HandleStatement (HandleContext context)
    {
      AssignmentStatement assignment = (AssignmentStatement) context.Statement;
      if(!CoversAssignment(assignment))
        return;

      Method sourceConstructor = ExtractConstructor (assignment.Source);
      string variableName = IntrospectionUtility.GetVariableName (assignment.Target);
      bool fragmentTypeInferenceRequired = FirstParameterIsString (sourceConstructor);
      
      if (fragmentTypeInferenceRequired)
      {
        Fragment fragmentType = FirstOperandsFragmentType(assignment, context);
        context.StringBuilderFragmentTypeDefined[variableName] = true;
        context.SymbolTable.MakeSafe (variableName, fragmentType);
      }
      else
      {
        //context.StringBuilderFragmentTypeDefined[variableName] = false;
        context.SymbolTable.MakeSafe (variableName, Fragment.CreateUndefined());
      }
    }
    
    protected override bool CoversAssignment (AssignmentStatement assignmentStatement)
    {
      Method sourceConstructor = ExtractConstructor (assignmentStatement.Source);
      bool sourceIsStringBuilderConstructor = sourceConstructor != null && _stringBuilderConstructorFullnames.Contains(sourceConstructor.FullName);
      return IntrospectionUtility.IsVariable (assignmentStatement.Target) && sourceIsStringBuilderConstructor;
    }

    private Method ExtractConstructor (Expression expression)
    {
      Method constructor = null;

      if (expression is Construct)
      {
        Construct construct = (Construct) expression;
        if (construct.Constructor is MemberBinding)
        {
          MemberBinding memberBinding = (MemberBinding) construct.Constructor;
          if (memberBinding.BoundMember is Method)
          {
            constructor = (Method) memberBinding.BoundMember;
          }
        }
      }

      return constructor;
    }
    
    private bool FirstParameterIsString (Method sourceConstructor)
    {
      bool firstParameterIsString = false;
      if (sourceConstructor.Parameters.Count >= 1)
      {
        firstParameterIsString = sourceConstructor.Parameters[0].Type.FullName == "System.String";
      }
      return firstParameterIsString;
    }

    private Fragment FirstOperandsFragmentType (AssignmentStatement assignment, HandleContext context)
    {
      Fragment fragmentType = Fragment.CreateEmpty();
      Expression firstOperand = FirstOperand (assignment.Source);
      if (firstOperand != null)
      {
        ISymbolTable symbolTable = context.SymbolTable;
        fragmentType = symbolTable.InferFragmentType(firstOperand);
      }
      return fragmentType;
    }

    private Expression FirstOperand (Expression source)
    {
      Expression firstOperand = null;
      if (source is Construct)
      {
        Construct construct = (Construct) source;
        if (construct.Operands.Count >= 1)
          firstOperand = construct.Operands[0];
      }
      return firstOperand;
    }
  }
}
