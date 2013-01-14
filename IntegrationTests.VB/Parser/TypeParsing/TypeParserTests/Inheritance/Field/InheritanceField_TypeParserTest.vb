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
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.Field
	<TestFixture()>
	Public Class InheritanceField_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_UnsafeAssignmentOnInheritedField_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOnInheritedField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnInheritedFieldWithLiteral_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnInheritedFieldWithLiteral", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnInheritedField_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnInheritedField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnFieldSetByHidingParent_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnFieldSetByHidingParent", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentOnFieldResetByHidingParent_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOnFieldResetByHidingParent", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallWithInheritedField_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallWithInheritedField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallWithInheritedField_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallWithInheritedField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallWithFieldSetByHidingParentField_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeCallWithFieldSetByHidingParentField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallWithFieldSetByHidingParentField_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeCallWithFieldSetByHidingParentField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOnBaseField_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOnBaseField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentOnBaseField_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOnBaseField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeAssignmentOfBaseField_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeAssignmentOfBaseField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeAssignmentOfBaseField_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeAssignmentOfBaseField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeMethodCallUsingBaseField_NoProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "SafeMethodCallUsingBaseField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeMethodCallUsingBaseField_ReturnsProblem()
			Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of InheritanceSampleField)()
			Dim sample As Method = IntrospectionUtility.MethodFactory(sampleTypeNode, "UnsafeMethodCallUsingBaseField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
