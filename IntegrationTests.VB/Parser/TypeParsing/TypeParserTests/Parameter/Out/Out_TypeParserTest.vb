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

Namespace Parser.TypeParsing.TypeParserTests.Parameter.Out
	<TestFixture()>
	Public Class Out_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_FragmentOutParameterSafe_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("FragmentOutParameterSafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentOutParameterUnsafe_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("FragmentOutParameterUnsafeCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutParameterSafeOperand_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("OutParameterSafeOperand", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_OutParameterUnsafeOperand_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("OutParameterUnsafeOperand", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_OutParameterSafeVariableTurningUnsafe_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("OutParameterSafeVariableTurningUnsafe", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_FragmentOutParameterSafeReturn_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("FragmentOutParameterSafeReturn", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentOutParameterUnsafeReturn_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("FragmentOutParameterUnsafeReturn", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_FragmentOutParameterSafeReturnWithAssignment_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("FragmentOutParameterSafeReturnWithAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_FragmentOutParameterUnsafeReturnWithAssignment_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("FragmentOutParameterUnsafeReturnWithAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeFragmentOutParameterInsideCondition_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("SafeFragmentOutParameterInsideCondition", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeFragmentOutParameterInsideCondition_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("UnsafeFragmentOutParameterInsideCondition", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeFragmentOutParameterInsideWhile_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("SafeFragmentOutParameterInsideWhile", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Ignore("Möglicherweise Problem mit Assignment"), Test()>
		Public Sub Parse_UnsafeFragmentOutParameterInsideWhile_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("UnsafeFragmentOutParameterInsideWhile", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeReturnWithAssignment_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of OutSample)("UnsafeReturnWithAssignment", New TypeNode() { stringTypeNode.GetReferenceType() })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
