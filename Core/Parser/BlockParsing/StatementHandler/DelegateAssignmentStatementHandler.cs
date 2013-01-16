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
using Microsoft.FxCop.Sdk;
using InjectionCop.Utilities;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Config;


namespace InjectionCop.Parser.BlockParsing.StatementHandler
{
  public class DelegateAssignmentStatementHandler: StatementHandlerBase<AssignmentStatement>
  {
    public DelegateAssignmentStatementHandler (
        IProblemPipe problemPipe,
        string returnFragmentType,
        List<ReturnCondition> returnConditions,
        IBlacklistManager blacklistManager,
        BlockParser.InspectCallback inspect)
        : base (problemPipe, returnFragmentType, returnConditions, blacklistManager, inspect)
    {
    }

    protected override void HandleStatement(Statement statement, ISymbolTable symbolTable, List<IPreCondition> preConditions, List<string> assignmentTargetVariables, List<BlockAssignment> blockAssignments, List<int> successors)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) statement;
      Construct construct = (Construct)assignmentStatement.Source;
      var expression = construct.Operands[1];
      MemberBinding methodBinding;
    
      if (expression is UnaryExpression)
        methodBinding = (MemberBinding)((UnaryExpression)expression).Operand;
      else if (expression is BinaryExpression) 
        //vb.net generates binaryExpressions instead of unary
        methodBinding = (MemberBinding) ((BinaryExpression) expression).Operand2;
      else
        throw new InvalidOperationException ("Could not fetch member binding from delegate statement.");
        
      Method assignedMethod = (Method)methodBinding.BoundMember;

      DelegateNode sourceDelegate = (DelegateNode)assignmentStatement.Source.Type;
      string returnFragment = SymbolTable.EMPTY_FRAGMENT;
      foreach (Member member in sourceDelegate.Members)
      {
        if (member.Name.Name == "Invoke")
        {
          Method invoke = (Method)member;
          returnFragment = FragmentUtility.ReturnFragmentType(invoke);
        }
      }

      ISymbolTable environment = new SymbolTable(_blacklistManager);
      foreach (Parameter parameter in sourceDelegate.Parameters)
      {
        if (parameter.Attributes != null)
        {
          environment.MakeSafe(parameter.Name.Name, FragmentUtility.GetFragmentType(parameter.Attributes));
        }
      }

      IMethodGraphAnalyzer methodParser = new MethodGraphAnalyzer(_problemPipe);
      IMethodGraphBuilder methodGraphBuilder = new MethodGraphBuilder(assignedMethod, _blacklistManager, _problemPipe, returnFragment);
      IInitialSymbolTableBuilder parameterSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(assignedMethod, _blacklistManager, environment);
      methodParser.Parse(methodGraphBuilder, parameterSymbolTableBuilder);
    }
  }
}
