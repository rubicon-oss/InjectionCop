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
Imports System.Linq

Namespace Parser.TypeParsing.TypeParserTests.AddProblem
	<TestFixture()>
	Public Class AddProblem_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub AddProblem_NoViolation_ReturnsNoProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("NoViolation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(0))
		End Sub

		<Test()>
		Public Sub AddProblem_OneViolation_ReturnsInjectionCopProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("OneViolation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub AddProblem_OneViolation_ReturnsOneProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("OneViolation", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub AddProblem_TwoViolations_ReturnsInjectionCopProblems()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("TwoViolations", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Dim returnsInjectionCopProblems As Boolean = result.All(Function(problem As Problem) problem.Id = Me.c_InjectionCopRuleId)
			Assert.That(returnsInjectionCopProblems, [Is].[True])
		End Sub

		<Test()>
		Public Sub AddProblem_TwoViolations_ReturnsTwoProblems()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("TwoViolations", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(2))
		End Sub

		<Test()>
		Public Sub AddProblem_OneViolationInWhile_ReturnsInjectionCopProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("OneViolationInWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub AddProblem_OneViolationInWhile_ReturnsOneProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("OneViolationInWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub AddProblem_OneViolationInWhileAssignmentAfterCall_ReturnsInjectionCopProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("OneViolationInWhileAssignmentAfterCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub AddProblem_OneViolationInWhileAssignmentAfterCall_ReturnsOneProblem()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("OneViolationInWhileAssignmentAfterCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(1))
		End Sub

		<Test()>
		Public Sub AddProblem_TwoViolationsInNestedWhile_ReturnsInjectionCopProblems()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("TwoViolationsInNestedWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Dim returnsInjectionCopProblems As Boolean = result.All(Function(problem As Problem) problem.Id = Me.c_InjectionCopRuleId)
			Assert.That(returnsInjectionCopProblems, [Is].[True])
		End Sub

		<Test()>
		Public Sub AddProblem_TwoViolationsInNestedWhile_ReturnsTwoProblems()
			Dim sample As Method = TestHelper.GetSample(Of AddProblemSample)("TwoViolationsInNestedWhile", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(result.Count, [Is].EqualTo(2))
		End Sub
	End Class
End Namespace
