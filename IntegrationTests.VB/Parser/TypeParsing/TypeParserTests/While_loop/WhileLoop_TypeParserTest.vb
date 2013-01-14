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

Namespace Parser.TypeParsing.TypeParserTests.While_loop
	<TestFixture()>
	Public Class WhileLoop_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_ValidCallInsideWhile_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("ValidCallInsideWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InValidCallInsideWhile_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InValidCallInsideWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InValidCallInsideWhileReprocessingRequired_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InValidCallInsideWhileReprocessingRequired", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InValidAssignmentInsideWhileReprocessingRequired_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InValidAssignmentInsideWhileReprocessingRequired", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideNestedWhile_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InvalidCallInsideNestedWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InValidCallInsideNestedWhileReprocessingRequired_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InValidCallInsideNestedWhileReprocessingRequired", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InValidCallInsideDeeperNestedWhileReprocessingRequired_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InValidCallInsideDeeperNestedWhileReprocessingRequired", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidCallInsideWhileWithContinue_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("ValidCallInsideWhileWithContinue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideWhileWithContinue_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InvalidCallInsideWhileWithContinue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideIfWithContinue_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InvalidCallInsideIfWithContinue", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidCallInsideWhileWithBreak_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("ValidCallInsideWhileWithBreak", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideWhileWithBreak_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InvalidCallInsideWhileWithBreak", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidCallInsideIfWithBreak_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InvalidCallInsideIfWithBreak", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InValidCallInsideWhileCondition_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of WhileLoopSample)("InValidCallInsideWhileCondition", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
