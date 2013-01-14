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

    Public Shadows Function NonVirtualMethod(<Fragment("InheritanceFragment")> annotatedParameter As String, <Fragment("InheritanceFragment")> nonAnnotatedParameter As String) As String
      Return "dummy"
    End Function

    Public Sub SafeCallOnInheritedMethod()
      InvariantMethod("safe", "safe")
    End Sub

    Public Sub UnsafeCallOnInheritedMethod()
      InvariantMethod(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
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
