Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.MemberMethod
  Public Class InheritanceSampleMethod
    Inherits InheritanceSampleBase

    Public Class ExtendedSample
      Inherits InheritanceSampleBase


      Public Sub New()
        MyBase.New("", "")
      End Sub
    End Class

    Public Class ExtendedExtendedSample
      Inherits InheritanceSampleMethod.ExtendedSample

    End Class

    Public Sub New()
      MyBase.New("safe", "safe")
    End Sub

    Public Overrides Function VirtualMethod(<Fragment("InheritanceFragment")> annotatedParameter As String, nonAnnotatedParameter As String) As String
      Return "dummy"
    End Function

    Public Function NonVirtualMethod(<Fragment("InheritanceFragment")> annotatedParameter As String, <Fragment("InheritanceFragment")> nonAnnotatedParameter As String) As String
      Return "dummy"
    End Function

    Public Sub SafeCallOnInheritedMethod()
      MyBase.InvariantMethod("safe", "safe")
    End Sub

    Public Sub UnsafeCallOnInheritedMethod()
      MyBase.InvariantMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
    End Sub

    Public Sub SafeCallOnMethodInheritedFromSuperiorClass()
      Dim sample As InheritanceSampleMethod.ExtendedExtendedSample = New InheritanceSampleMethod.ExtendedExtendedSample()
      sample.InvariantMethod("safe", "safe")
    End Sub

    Public Sub UnsafeCallOnMethodInheritedFromSuperiorClass()
      Dim sample As InheritanceSampleMethod.ExtendedExtendedSample = New InheritanceSampleMethod.ExtendedExtendedSample()
      sample.InvariantMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
    End Sub

    Public Sub SafeCallOnNewMethod()
      Me.NonVirtualMethod("safe", "safe")
    End Sub

    Public Sub UnsafeCallOnNewMethod()
      Me.NonVirtualMethod("safe", InheritanceSampleBase.UnsafeInheritanceFragmentSource())
    End Sub

    Public Sub SafeStaticBindingOnNewMethod()
      Dim sample As InheritanceSampleBase = New InheritanceSampleMethod()
      sample.NonVirtualMethod("", "")
    End Sub

    Public Sub UnsafeStaticBindingOnNewMethod()
      Dim sample As InheritanceSampleBase = New InheritanceSampleMethod()
      sample.NonVirtualMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "")
    End Sub

    Public Sub SafeCallBaseMethod()
      MyBase.NonVirtualMethod("safe", "safe")
    End Sub

    Public Sub UnsafeCallBaseMethod()
      MyBase.NonVirtualMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
    End Sub

    Public Sub SafeCallOnOverriddenMethod()
      Me.VirtualMethod("safe", "safe")
    End Sub

    Public Sub AnotherSafeCallOnOverriddenMethod()
      Me.VirtualMethod("safe", InheritanceSampleBase.UnsafeInheritanceFragmentSource())
    End Sub

    Public Sub UnsafeCallOnOverriddenMethod()
      Me.VirtualMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
    End Sub

    Public Sub SafeDynamicBinding()
      Dim sample As InheritanceSampleBase = New InheritanceSampleMethod()
      sample.VirtualMethod("safe", "safe")
    End Sub

    Public Sub UnsafeDynamicBinding()
      Dim sample As InheritanceSampleBase = New InheritanceSampleMethod()
      sample.VirtualMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
    End Sub
  End Class
End Namespace
