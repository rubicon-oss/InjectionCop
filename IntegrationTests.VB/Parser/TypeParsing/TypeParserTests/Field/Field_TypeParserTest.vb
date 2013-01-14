' Copyright 2013 rubicon informationstechnologie gmbh
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
' http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Field
	<TestFixture()>
	Public Class Field_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_CallWithSafeField_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("CallWithSafeField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_CallWithUnsafeField_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("CallWithUnsafeField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_CallWithWrongFragmentType_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("CallWithWrongFragmentType", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_NestedUnsafeCall_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("NestedUnsafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_NestedSafeCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("NestedSafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_NestedCallWithWrongFragmentType_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("NestedCallWithWrongFragmentType", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeFieldAssignment_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("UnsafeFieldAssignment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeFieldAssignment_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("SafeFieldAssignment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeFieldAssignmentWithLiteral_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("SafeFieldAssignmentWithLiteral", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeFieldAssignmentWithField_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("UnsafeFieldAssignmentWithField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeFieldAssignmentWithField_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("SafeFieldAssignmentWithField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_WrongFragmentTypeFieldAssignment_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of FieldSample)("WrongFragmentTypeFieldAssignment", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
