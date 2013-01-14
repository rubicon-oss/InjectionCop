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
