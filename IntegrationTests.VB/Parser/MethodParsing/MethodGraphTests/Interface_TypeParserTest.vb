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
Imports InjectionCop.Parser.MethodParsing
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Parser.MethodParsing.MethodGraphTests
	<TestFixture()>
	Public Class Interface_TypeParserTest
		Inherits MethodGraph_TestBase

		<Test()>
		Public Sub IsEmpty_MethodNonAnnotated_ReturnsTrue()
			Dim objectTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(Object))
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(GetType(String))
			Dim sampleMethod As Method = IntrospectionUtility.MethodFactory(GetType(InterfaceSample), "MethodNonAnnotated", New TypeNode() { objectTypeNode, stringTypeNode })
			Dim methodGraph As IMethodGraph = MyBase.BuildMethodGraph(sampleMethod)
			Assert.That(methodGraph, [Is].Null)
		End Sub
	End Class
End Namespace
