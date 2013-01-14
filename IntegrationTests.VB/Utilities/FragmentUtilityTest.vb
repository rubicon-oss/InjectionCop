Imports InjectionCop.IntegrationTests.VB.Parser
Imports InjectionCop.Parser
Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System

Namespace Utilities
  <TestFixture()>
  Public Class FragmentUtilityTest
    <Test()>
    Public Sub IsFragment_ContainsFragmentParameter_True()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsFragmentParameter", New TypeNode() {stringTypeNode})
      Dim isResult As Boolean = FragmentUtility.IsFragment(sample.Parameters(0).Attributes(0))
      Assert.That(isResult, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsFragment_ContainsNonFragmentParameter_False()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsNonFragmentParameter", New TypeNode() {stringTypeNode})
      Dim isResult As Boolean = FragmentUtility.IsFragment(sample.Parameters(0).Attributes(0))
      Assert.That(isResult, [Is].[False])
    End Sub

    <Test()>
    Public Sub IsFragment_ContainsStronglyTypedSqlFragmentParameter_True()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsStronglyTypedSqlFragmentParameter", New TypeNode() {stringTypeNode})
      Dim isResult As Boolean = FragmentUtility.IsFragment(sample.Parameters(0).Attributes(0))
      Assert.That(isResult, [Is].[True])
    End Sub

    <Test()>
    Public Sub ContainsFragment_ContainsFragmentParameter_True()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsFragmentParameter", New TypeNode() {stringTypeNode})
      Dim isResult As Boolean = FragmentUtility.ContainsFragment(sample.Parameters(0).Attributes)
      Assert.That(isResult, [Is].[True])
    End Sub

    <Test()>
    Public Sub ContainsFragment_ContainsNonFragmentParameter_False()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsNonFragmentParameter", New TypeNode() {stringTypeNode})
      Dim isResult As Boolean = FragmentUtility.ContainsFragment(sample.Parameters(0).Attributes)
      Assert.That(isResult, [Is].[False])
    End Sub

    <Test()>
    Public Sub ContainsFragment_ContainsStronglyTypedSqlFragmentParameter_True()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsStronglyTypedSqlFragmentParameter", New TypeNode() {stringTypeNode})
      Dim isResult As Boolean = FragmentUtility.ContainsFragment(sample.Parameters(0).Attributes)
      Assert.That(isResult, [Is].[True])
    End Sub

    <Test()>
    Public Sub GetFragmentType_ContainsFragmentParameter_ReturnsType()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsFragmentParameter", New TypeNode() {stringTypeNode})
      Dim fragmentType As String = FragmentUtility.GetFragmentType(sample.Parameters(0).Attributes)
      Assert.That(fragmentType, [Is].EqualTo("FragmentType"))
    End Sub

    <Test()>
    Public Sub GetFragmentType_ContainsStronglyTypedSqlFragmentParameter_True()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsStronglyTypedSqlFragmentParameter", New TypeNode() {stringTypeNode})
      Dim fragmentType As String = FragmentUtility.GetFragmentType(sample.Parameters(0).Attributes)
      Assert.That(fragmentType, [Is].EqualTo("SqlFragment"))
    End Sub

    <Test()>
    Public Sub GetFragmentType_ContainsNonFragmentParameter_ThrowsException()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ContainsNonFragmentParameter", New TypeNode() {stringTypeNode})
      Dim returnedFragment As String = FragmentUtility.GetFragmentType(sample.Parameters(0).Attributes)
      Assert.That(returnedFragment, [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
    End Sub

    <Test()>
    Public Sub ReturnFragmentType_NonAnnotatedMethod_ReturnsEmptyFragment()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("NoReturnFragment", New TypeNode() {})
      Dim returnFragment As String = FragmentUtility.ReturnFragmentType(sample)
      Assert.That(returnFragment, [Is].EqualTo(SymbolTable.EMPTY_FRAGMENT))
    End Sub

    <Test()>
    Public Sub ReturnFragmentType_MethodWithAnnotatedReturn_ReturnsNull()
      Dim sample As Method = TestHelper.GetSample(Of FragmentUtilitySample)("ReturnFragment", New TypeNode() {})
      Dim returnFragment As String = FragmentUtility.ReturnFragmentType(sample)
      Assert.That(returnFragment, [Is].EqualTo("ReturnFragmentType"))
    End Sub

    <Test()>
    Public Sub ReturnFragmentType_ImplementedInterfaceMethod_ReturnsFragment()
      Dim sample As Method = TestHelper.GetSample(Of ClassWithMethodReturningFragment)("MethodWithReturnFragment", New TypeNode() {})
      Dim returnFragment As String = FragmentUtility.ReturnFragmentType(sample)
      Assert.That(returnFragment, [Is].EqualTo("ReturnFragmentType"))
    End Sub

    <Test()>
    Public Sub ReturnFragmentType_ExplicitlyImplementedInterfaceMethod_ReturnsFragment()
      Dim sample As Method = TestHelper.GetSample(Of ClassWithExplicitlyDeclaredMethodReturningFragment)("InjectionCop.IntegrationTests.Utilities.InterfaceWithReturnFragment.MethodWithReturnFragment", New TypeNode() {})
      Dim returnFragment As String = FragmentUtility.ReturnFragmentType(sample)
      Assert.That(returnFragment, [Is].EqualTo("ReturnFragmentType"))
    End Sub
  End Class
End Namespace
