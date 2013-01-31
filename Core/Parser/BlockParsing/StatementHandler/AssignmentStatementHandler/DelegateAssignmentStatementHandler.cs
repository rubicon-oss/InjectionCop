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
using Microsoft.FxCop.Sdk;
using InjectionCop.Utilities;
using InjectionCop.Parser.MethodParsing;

namespace InjectionCop.Parser.BlockParsing.StatementHandler.AssignmentStatementHandler
{
  public class DelegateAssignmentStatementHandler : StatementHandlerBase<AssignmentStatement>
  {
    public DelegateAssignmentStatementHandler (BlockParserContext blockParserContext)
        : base (blockParserContext)
    {
    }

    protected override void HandleStatement (HandleContext context)
    {
      AssignmentStatement assignmentStatement = (AssignmentStatement) context.Statement;
      Method assignedMethod = GetAssignedDelegateMethod (assignmentStatement);
      DelegateNode sourceDelegateType = (DelegateNode) assignmentStatement.Source.Type;
      Fragment returnFragment = GetDelegateTypesReturnFragment (sourceDelegateType);
      ISymbolTable environment = GetDelegatesEnvironment (sourceDelegateType);

      IMethodGraphAnalyzer methodParser = new MethodGraphAnalyzer (_blockParserContext.ProblemPipe);
      IMethodGraphBuilder methodGraphBuilder = new MethodGraphBuilder (assignedMethod, _blockParserContext.BlacklistManager, _blockParserContext.ProblemPipe, returnFragment);
      IInitialSymbolTableBuilder parameterSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder (assignedMethod, _blockParserContext.BlacklistManager, environment);
      methodParser.Parse (methodGraphBuilder, parameterSymbolTableBuilder);
    }

    private Method GetAssignedDelegateMethod (AssignmentStatement assignmentStatement)
    {
      Construct construct = (Construct) assignmentStatement.Source;
      var expression = construct.Operands[1];
      MemberBinding methodBinding;

      if (expression is UnaryExpression)
        methodBinding = (MemberBinding) ((UnaryExpression) expression).Operand;
      else if (expression is BinaryExpression)
          //vb.net generates binaryExpressions instead of unary
        methodBinding = (MemberBinding) ((BinaryExpression) expression).Operand2;
      else
        throw new InvalidOperationException ("Could not fetch member binding from delegate statement.");

      return (Method) methodBinding.BoundMember;
    }

    private Fragment GetDelegateTypesReturnFragment (DelegateNode sourceDelegateType)
    {
      var returnFragment = Fragment.CreateEmpty();
      foreach (Member member in sourceDelegateType.Members)
      {
        if (member.Name.Name == "Invoke")
        {
          Method invoke = (Method) member;
          returnFragment = FragmentUtility.ReturnFragmentType (invoke);
        }
      }
      return returnFragment;
    }

    private ISymbolTable GetDelegatesEnvironment (DelegateNode sourceDelegateType)
    {
      ISymbolTable environment = new SymbolTable (_blockParserContext.BlacklistManager);
      foreach (Parameter parameter in sourceDelegateType.Parameters)
      {
        if (parameter.Attributes != null)
        {
          environment.MakeSafe (parameter.Name.Name, FragmentUtility.GetFragmentType (parameter.Attributes));
        }
      }
      return environment;
    }
  }
}
