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

Namespace Utilities
	<TestFixture()>
	Public Class IntrospectionUtility_InterfaceTest
		<Test()>
		Public Sub TypeNodeFactory_Interface_ReturnsInterfaceTypeNode()
			Dim sampleType As System.Type = GetType(IntrospectionUtility_InterfaceSample)
			Dim result As TypeNode = IntrospectionUtility.TypeNodeFactory(sampleType)
			Assert.That(result.FullName, [Is].EqualTo(sampleType.FullName))
		End Sub

		<ExpectedException(GetType(System.ArgumentNullException)), Test()>
		Public Sub TypeNodeFactory_Null_ReturnsException()
			IntrospectionUtility.TypeNodeFactory(Nothing)
		End Sub

		<ExpectedException(GetType(System.ArgumentNullException)), Test()>
		Public Sub MethodFactory_TypeNull_ReturnsException()
      IntrospectionUtility.MethodFactory(CType(Nothing, Type), "methodname", New TypeNode() {})
		End Sub

		<ExpectedException(GetType(System.ArgumentNullException)), Test()>
		Public Sub MethodFactory_MethodNameNull_ReturnsException()
			IntrospectionUtility.MethodFactory(GetType(IntrospectionUtility_InterfaceSample), Nothing, New TypeNode() {})
		End Sub

		<ExpectedException(GetType(System.ArgumentNullException)), Test()>
		Public Sub MethodFactory_MethodWithParametersParametersNull_ReturnsException()
			IntrospectionUtility.MethodFactory(GetType(IntrospectionUtility_InterfaceSample), "MethodWithParameter", Nothing)
		End Sub

		<ExpectedException(GetType(System.ArgumentNullException)), Test()>
		Public Sub MethodFactory_MethodWithoutParametersParametersNull_ReturnsException()
			IntrospectionUtility.MethodFactory(GetType(IntrospectionUtility_InterfaceSample), "MethodWithoutParameters", Nothing)
		End Sub

		<Test()>
		Public Sub MethodFactory_MethodWithoutParametersCorrectCall_ReturnsMethod()
			Dim sampleType As System.Type = GetType(IntrospectionUtility_InterfaceSample)
			Dim sampleMethodname As String = "MethodWithoutParameters"
			Dim result As Method = IntrospectionUtility.MethodFactory(sampleType, sampleMethodname, New TypeNode() {})
			Dim correctType As Boolean = result.DeclaringType.FullName = sampleType.FullName
			Dim correctMethod As Boolean = result.Name.Name = sampleMethodname
			Assert.That(correctMethod AndAlso correctType, [Is].[True])
		End Sub

		<Test()>
		Public Sub MethodFactory_MethodWithParametersCorrectCall_ReturnsMethod()
			Dim sampleType As System.Type = GetType(IntrospectionUtility_InterfaceSample)
			Dim intTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Integer)()
			Dim sampleMethodname As String = "MethodWithParameter"
			Dim result As Method = IntrospectionUtility.MethodFactory(sampleType, sampleMethodname, New TypeNode() { intTypeNode })
			Dim correctType As Boolean = result.DeclaringType.FullName = sampleType.FullName
			Dim correctMethod As Boolean = result.Name.Name = sampleMethodname
			Assert.That(correctMethod AndAlso correctType, [Is].[True])
		End Sub

		<Test()>
		Public Sub MethodFactory_MethodWithParametersWrongParameterType_ReturnsNull()
			Dim sampleType As System.Type = GetType(IntrospectionUtility_InterfaceSample)
			Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
			Dim sampleMethodname As String = "MethodWithParameter"
			Dim result As Method = IntrospectionUtility.MethodFactory(sampleType, sampleMethodname, New TypeNode() { stringTypeNode })
			Assert.That(result, [Is].Null)
		End Sub
	End Class
End Namespace
