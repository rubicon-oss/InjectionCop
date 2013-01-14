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

Namespace Parser.TypeParsing.TypeParserTests.Switch
	<TestFixture()>
	Public Class Switch_TypeParserTest
		Inherits TypeParserTestBase

		<Test()>
		Public Sub Parse_ValidSwitch_NoProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("ValidSwitch", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideSwitch_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("UnsafeCallInsideSwitch", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallAfterSwitch_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("UnsafeCallAfterSwitch", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallAfterNestedSwitch_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("UnsafeCallAfterNestedSwitch", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_SafeCallAfterNestedSwitch_NoProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("SafeCallAfterNestedSwitch", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_UnsafeCallInsideNestedSwitch_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("UnsafeCallInsideNestedSwitch", New TypeNode() { intTypeNode, intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_ValidFallThrough_NoProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("ValidFallThrough", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_ValidFallThroughGoto_NoProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("ValidFallThroughGoto", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub

		<Test()>
		Public Sub Parse_InvalidFallThrough_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("InvalidFallThrough", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub

		<Test()>
		Public Sub Parse_InvalidFallThroughGoto_ReturnsProblem()
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sample As Method = TestHelper.GetSample(Of SwitchSample)("InvalidFallThroughGoto", New TypeNode() { intTypeNode })
			Me._typeParser.Parse(sample)
			Dim result As ProblemCollection = Me._typeParser.Problems
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[True])
		End Sub
	End Class
End Namespace
