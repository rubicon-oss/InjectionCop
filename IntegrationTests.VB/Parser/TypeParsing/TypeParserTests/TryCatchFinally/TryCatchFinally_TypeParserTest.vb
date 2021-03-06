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

Namespace Parser.TypeParsing.TypeParserTests.TryCatchFinally
	<TestFixture()>
	Public Class TryCatchFinally_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeCallInsideTry_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("SafeCallInsideTry", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideTry_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallInsideTry", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallInsideCatch_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("SafeCallInsideCatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideCatch_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallInsideCatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallInsideFinally_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("SafeCallInsideFinally", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideFinally_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallInsideFinally", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallNestedTry_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallNestedTry", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallNestedCatch_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallNestedCatch", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallNestedFinally_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of TryCatchFinallySample)("UnsafeCallNestedFinally", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
