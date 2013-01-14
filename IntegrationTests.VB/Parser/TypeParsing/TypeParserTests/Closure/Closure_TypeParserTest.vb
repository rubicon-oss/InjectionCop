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

Namespace Parser.TypeParsing.TypeParserTests.Closure
	<Ignore("Closures are not suppoted in version 1.0"), TestFixture()>
	Public Class Closure_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_SafeClosureUsingLocalVariable_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of ClosureSample)("SafeClosureUsingLocalVariable", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeClosureUsingLocalVariable_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of ClosureSample)("UnsafeClosureUsingLocalVariable", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeClosureUsingField_NoProblem()
			Dim sample As Method = TestHelper.GetSample(Of ClosureSample)("SafeClosureUsingField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeClosureUsingField_ReturnsProblem()
			Dim sample As Method = TestHelper.GetSample(Of ClosureSample)("UnsafeClosureUsingField", New TypeNode() {})
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
