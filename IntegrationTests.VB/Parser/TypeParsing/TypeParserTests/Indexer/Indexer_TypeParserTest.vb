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

Namespace Parser.TypeParsing.TypeParserTests.Indexer
	<TestFixture()>
	Public Class Indexer_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_IndexerSafeAssignmentOnParameter_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("IndexerSafeAssignmentOnParameter", New TypeNode() { stringTypeNode.GetArrayType(1) })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_IndexerUnsafeAssignmentOnParameter_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("IndexerUnsafeAssignmentOnParameter", New TypeNode() { stringTypeNode.GetArrayType(1) })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallUsingIndexer_NoProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("SafeCallUsingIndexer", New TypeNode() { stringTypeNode.GetArrayType(1) })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallUsingIndexer_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("UnsafeCallUsingIndexer", New TypeNode() { stringTypeNode.GetArrayType(1) })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallWithElementSetUnsafeByIndexer_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("UnsafeCallWithElementSetUnsafeByIndexer", New TypeNode() { stringTypeNode.GetArrayType(1) })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallWithSingleVariableSetSafeByIndexer_ReturnsProblem()
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("UnsafeCallWithSingleVariableSetSafeByIndexer", New TypeNode() { stringTypeNode.GetArrayType(1) })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallUsingArray_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of IndexerSample)("SafeCallUsingArray", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
