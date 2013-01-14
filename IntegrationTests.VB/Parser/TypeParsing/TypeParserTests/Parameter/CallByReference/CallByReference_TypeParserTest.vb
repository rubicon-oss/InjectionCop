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

Namespace Parser.TypeParsing.TypeParserTests.Parameter.CallByReference
	<TestFixture()>
	Public Class CallByReference_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_FragmentRefParameterSafe_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("FragmentRefParameterSafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentRefParameterUnsafe_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("FragmentRefParameterUnsafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefParameterSafeOperand_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("RefParameterSafeOperand", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_RefParameterUnsafeOperand_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("RefParameterUnsafeOperand", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_RefParameterSafeVariableTurningUnsafe_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("RefParameterSafeVariableTurningUnsafe", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_FragmentRefParameterSafeReturn_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("FragmentRefParameterSafeReturn", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentRefParameterUnsafeReturn_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("FragmentRefParameterUnsafeReturn", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_FragmentRefParameterSafeReturnWithAssignment_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("FragmentRefParameterSafeReturnWithAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentRefParameterUnsafeReturnWithAssignment_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("FragmentRefParameterUnsafeReturnWithAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeFragmentRefParameterInsideCondition_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("SafeFragmentRefParameterInsideCondition", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeFragmentRefParameterInsideCondition_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("UnsafeFragmentRefParameterInsideCondition", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeFragmentRefParameterInsideWhile_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("SafeFragmentRefParameterInsideWhile", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Ignore("Möglicherweise Problem mit assignment"), Test()>
		Public Sub Parse_UnsafeFragmentRefParameterInsideWhile_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("UnsafeFragmentRefParameterInsideWhile", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeReturnWithAssignment_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of CallByReferenceSample)("UnsafeReturnWithAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
