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

Namespace Parser.TypeParsing.TypeParserTests.Attribute
	<TestFixture()>
	Public Class Attribute_TypeParserTest
		Inherits TypeParserTestBase

		<Category("Attribute"), Test()>
		Public Sub Parse_ParameterSampleType_NoProblem()
			Dim sample As TypeNode = IntrospectionUtility.TypeNodeFactory(Of SampleAttribute)()
			Dim result As ProblemCollection = Me._typeParser.Check(sample)
			Assert.That(TestHelper.ContainsProblemID(Me.c_InjectionCopRuleId, result), [Is].[False])
		End Sub
	End Class
End Namespace
