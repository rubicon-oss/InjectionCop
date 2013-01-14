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

Namespace Parser.TypeParsing.TypeParserTests.BlackMethod
	<TestFixture()>
	Public Class BlackMethod_BlockParserTest
		Inherits TypeParserTestBase

		<Category("BlackMethod"), Test()>
		Public Sub Parse_BlackMethodCallLiteral_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlackMethodSample)("BlackMethodCallLiteral", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Category("BlackMethod"), Ignore(), Test()>
		Public Sub Parse_BlackMethodCallUnsafeSourceNoParameter_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlackMethodSample)("BlackMethodCallUnsafeSourceNoParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Category("BlackMethod"), Test()>
		Public Sub Parse_BlackMethodCallSafeSource_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlackMethodSample)("BlackMethodCallSafeSource", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Category("BlackMethod"), Ignore(), Test()>
		Public Sub Parse_BlackMethodCallUnsafeSourceWithSafeParameter_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlackMethodSample)("BlackMethodCallUnsafeSourceWithSafeParameter", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Category("BlackMethod"), Test()>
		Public Sub Parse_WhiteMethodCall_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of BlackMethodSample)("WhiteMethodCall", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
