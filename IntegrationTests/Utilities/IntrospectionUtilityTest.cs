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
using InjectionCop.IntegrationTests.Parser;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Utilities
{
  [TestFixture]
  public class IntrospectionUtilityTest
  {
    [Test]
    public void IsVariable_FieldInMembebinding_IsTrue ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample>("UsingField");
      Block expressionBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)expressionBlock.Statements[1];
      UnaryExpression sampleUnaryExpression = (UnaryExpression)expressionStatement.Expression;
      MethodCall sampleMethodCall = (MethodCall) sampleUnaryExpression.Operand;
      Expression sampleExpression = sampleMethodCall.Operands[0];
      bool fieldIsVariable = IntrospectionUtility.IsVariable(sampleExpression);
      Assert.That(fieldIsVariable, Is.True);
    }

    [Test]
    public void IsVariable_Method_IsFalse ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample>("UsingField");
      Block expressionBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)expressionBlock.Statements[1];
      UnaryExpression sampleUnaryExpression = (UnaryExpression)expressionStatement.Expression;
      MethodCall sampleMethodCall = (MethodCall) sampleUnaryExpression.Operand;
      bool methodIsVariable = IntrospectionUtility.IsVariable(sampleMethodCall);
      Assert.That(methodIsVariable, Is.False);
    }

    [Test]
    public void GetVariableName_FieldInMembebinding_ReturnsName ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample>("UsingField");
      Block expressionBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)expressionBlock.Statements[1];
      UnaryExpression sampleUnaryExpression = (UnaryExpression)expressionStatement.Expression;
      MethodCall sampleMethodCall = (MethodCall) sampleUnaryExpression.Operand;
      Expression sampleExpression = sampleMethodCall.Operands[0];
      string variableName = IntrospectionUtility.GetVariableName(sampleExpression);
      Assert.That(variableName, Is.EqualTo("_field"));
    }

    [Test]
    public void GetVariableName_Method_IsNull ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample>("UsingField");
      Block expressionBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)expressionBlock.Statements[1];
      UnaryExpression sampleUnaryExpression = (UnaryExpression)expressionStatement.Expression;
      MethodCall sampleMethodCall = (MethodCall) sampleUnaryExpression.Operand;
      string variableName = IntrospectionUtility.GetVariableName(sampleMethodCall);
      Assert.That(variableName, Is.Null);
    }

    [Test]
    public void IsVariableWithOutParameter_FieldInMemberbinding_IsTrueAndReturnsFieldName ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample>("UsingField");
      Block expressionBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)expressionBlock.Statements[1];
      UnaryExpression sampleUnaryExpression = (UnaryExpression)expressionStatement.Expression;
      MethodCall sampleMethodCall = (MethodCall) sampleUnaryExpression.Operand;
      Expression sampleExpression = sampleMethodCall.Operands[0];
      string variableName;
      bool fieldIsVariable = IntrospectionUtility.IsVariable(sampleExpression, out variableName);
      bool correctName = variableName == "_field";
      Assert.That(fieldIsVariable && correctName, Is.True);
    }

    [Test]
    public void IsVariableWithOutParameter_Method_IsFalseAndReturnsNullAsVariableName ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample>("UsingField");
      Block expressionBlock = (Block)sample.Body.Statements[0];
      ExpressionStatement expressionStatement = (ExpressionStatement)expressionBlock.Statements[1];
      UnaryExpression sampleUnaryExpression = (UnaryExpression)expressionStatement.Expression;
      MethodCall sampleMethodCall = (MethodCall) sampleUnaryExpression.Operand;
      string variableName;
      bool methodIsVariable = IntrospectionUtility.IsVariable(sampleMethodCall, out variableName);
      bool correctName = variableName == null;
      Assert.That(!methodIsVariable && correctName, Is.True);
    }

    [Test]
    public void IsField_FieldExpression_ReturnsTrue ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample> ("FieldAssignment");
      Block assignmentBlock = (Block) sample.Body.Statements[0];
      AssignmentStatement assignmentStatement = (AssignmentStatement) assignmentBlock.Statements[1];
      Expression sampleExpression = assignmentStatement.Target;
      bool fieldRecognized = IntrospectionUtility.IsField (sampleExpression);
      Assert.That (fieldRecognized, Is.True);
    }

    [Test]
    public void IsField_NonFieldAssignment_ReturnsFalse ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample> ("NonFieldAssignment");
      Block assignmentBlock = (Block) sample.Body.Statements[0];
      AssignmentStatement assignmentStatement = (AssignmentStatement) assignmentBlock.Statements[1];
      Expression sampleExpression = assignmentStatement.Target;
      bool fieldRecognized = IntrospectionUtility.IsField (sampleExpression);
      Assert.That (fieldRecognized, Is.False);
    }

    [Test]
    public void GetField_FieldExpression_ReturnsContainedField ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample> ("FieldAssignment");
      Block assignmentBlock = (Block) sample.Body.Statements[0];
      AssignmentStatement assignmentStatement = (AssignmentStatement) assignmentBlock.Statements[1];
      Expression sampleExpression = assignmentStatement.Target;
      Field extractedField = IntrospectionUtility.GetField(sampleExpression);

      Member boundMember = ((MemberBinding) sampleExpression).BoundMember;
      bool extractionCorrect = extractedField.UniqueKey == boundMember.UniqueKey;

      Assert.That (extractionCorrect, Is.True);
    }

    [Test]
    public void GetField_NonFieldAssignment_ReturnsNull ()
    {
      Method sample = TestHelper.GetSample<IntrospectionUtilitySample> ("NonFieldAssignment");
      Block assignmentBlock = (Block) sample.Body.Statements[0];
      AssignmentStatement assignmentStatement = (AssignmentStatement) assignmentBlock.Statements[1];
      Expression sampleExpression = assignmentStatement.Target;
      Field extractedField = IntrospectionUtility.GetField(sampleExpression);
      bool extractionCorrect = extractedField == null;

      Assert.That (extractionCorrect, Is.True);
    }
  }
}
