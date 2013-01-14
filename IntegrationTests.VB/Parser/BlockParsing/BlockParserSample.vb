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

Namespace Parser.BlockParsing
  Friend Class BlockParserSample
    Inherits ParserSampleBase

    Public Sub PostConditionOnlySafeSymbols()
      Dim x As Integer = Me.SafeSourceInt()
      Dim y As String = "safe"
      Me.doSomething(x, y)
    End Sub

    Public Sub PostConditionSafeAndUnsafeSymbols()
      Dim x As Integer = 0
      Dim y As String = MyBase.UnsafeSource()
      Me.doSomething(x, y)
    End Sub

    Public Sub UnsafePreCondition(unSafe As String)
      MyBase.RequiresSqlFragment(unSafe)
    End Sub

    Public Sub SafePreCondition(ignore As String)
      MyBase.RequiresSqlFragment("safe")
    End Sub

    Public Sub MultipleUnsafePreCondition(unSafe1 As String, unSafe2 As String)
      MyBase.RequiresSqlFragment(unSafe1)
      MyBase.RequiresSqlFragment(unSafe2)
    End Sub

    Public Sub BlockInternalSafenessCondition(x As String)
      Dim y As String = "Safe"
      MyBase.RequiresSqlFragment(x)
      MyBase.RequiresSqlFragment(y)
    End Sub

    Public Function ReturnFragmentRequiredLiteralReturn() As <Fragment("ReturnFragmentType")>
    String
      Return "dummy"
    End Function

    Public Function UnsafeReturnWhenFragmentRequired() As String
      Return MyBase.UnsafeSource()
    End Function

    Public Function UnsafeReturnWhenFragmentRequiredMoreComplex() As String
      Dim piIstGenauDrei As Integer = 3
      Dim dummy As String = "dummy"
      Me.doSomething(piIstGenauDrei, dummy)
      Return MyBase.UnsafeSource()
    End Function

    Public Sub DummyProcedure()
      Dim piIstGenauDrei As Integer = 3
      Dim dummy As String = "dummy"
      Me.doSomething(piIstGenauDrei, dummy)
    End Sub

    Public Function SetSuccessor(param As String) As String
      param += "dummy"
      Return param
    End Function

    Public Function doSomething(x As Integer, y As String) As String
      Return x + y
    End Function

    Public Function SafeSourceInt() As <Fragment("SqlFragment")>
    Integer
      Return 3
    End Function

    Public Function ValidReturnWithLiteralAssignmentInsideIf(<Fragment("DummyFragment")> parameter As String) As <Fragment("DummyFragment")>
    String
      Dim returnValue As String = ""
      If parameter = "Dummy" Then
        returnValue = "safe"
      End If
      Return returnValue
    End Function

    Public Function DeclarationWithReturn() As <Fragment("ReturnFragmentType")> Integer
      Dim i As Integer = 3
      Return i
    End Function

    Public Function ValidReturnWithParameterAssignmentInsideIf(<Fragment("DummyFragment")> parameter As String) As <Fragment("DummyFragment")>
    String
      Dim returnValue As String = ""
      If parameter = "Dummy" Then
        returnValue = parameter
      End If
      Return returnValue
    End Function

    Public Function ValidReturnWithParameterResetAndAssignmentInsideIf(<Fragment("DummyFragment")> parameter As String) As <Fragment("DummyFragment")>
    String
      Dim returnValue As String = ""
      If parameter = "Dummy" Then
        parameter = "safe"
        returnValue = parameter
      End If
      Return returnValue
    End Function

    Public Function ReturnLiteral() As String
      Return "dummy"
    End Function

    Public Sub OutReturnPreconditionCheckSafe(ByRef returnPreCondition As String)
      returnPreCondition = "safe"
    End Sub

    Public Sub OutReturnPreconditionCheckUnSafe(ByRef returnPreCondition As String)
      returnPreCondition = MyBase.UnsafeSource()
    End Sub

    Public Sub OutReturnPreconditionCheckSafeLiteralAssignment(ByRef returnPreCondition As String)
      returnPreCondition = MyBase.UnsafeSource()
      MyBase.DummyMethod(returnPreCondition)
      returnPreCondition = "safe"
    End Sub

    Public Sub OutReturnPreconditionConditional(ByRef returnPreCondition As String)
      Dim temp As String = "safe"
      returnPreCondition = MyBase.UnsafeSource()
      If returnPreCondition = "dummy" Then
        returnPreCondition = temp
      End If
    End Sub

    Public Sub OutReturnPreconditionConditionalWithReturnInsideIf(ByRef returnPreCondition As String)
      Dim temp As String = "safe"
      returnPreCondition = MyBase.UnsafeSource()
      If returnPreCondition = "dummy" Then
        returnPreCondition = temp
      Else
        MyBase.DummyMethod(returnPreCondition)
      End If
    End Sub

    Public Sub OutReturnPreconditionConditionalWithReturnAfterIf(ByRef returnPreCondition As String)
      Dim temp As String = "safe"
      returnPreCondition = MyBase.UnsafeSource()
      If returnPreCondition = "dummy" Then
        returnPreCondition = temp
        MyBase.DummyMethod(returnPreCondition)
      End If
      MyBase.DummyMethod(returnPreCondition)
    End Sub

    Public Sub RefReturnPreconditionCheckSafe(ByRef returnPreCondition As String)
      MyBase.DummyMethod(returnPreCondition)
      returnPreCondition = "safe"
    End Sub

    Public Sub RefReturnPreconditionCheckUnSafe(ByRef returnPreCondition As String)
      MyBase.DummyMethod(returnPreCondition)
      returnPreCondition = MyBase.UnsafeSource()
    End Sub

    Public Sub RefReturnPreconditionCheckSafeLiteralAssignment(ByRef returnPreCondition As String)
      MyBase.DummyMethod(returnPreCondition)
      returnPreCondition = MyBase.UnsafeSource()
      MyBase.DummyMethod(returnPreCondition)
      returnPreCondition = "safe"
    End Sub

    Public Sub RefReturnPreconditionConditional(ByRef returnPreCondition As String)
      MyBase.DummyMethod(returnPreCondition)
      Dim temp As String = "safe"
      returnPreCondition = MyBase.UnsafeSource()
      If returnPreCondition = "dummy" Then
        returnPreCondition = temp
      End If
    End Sub

    Public Sub RefReturnPreconditionConditionalWithReturnInsideIf(ByRef returnPreCondition As String)
      MyBase.DummyMethod(returnPreCondition)
      Dim temp As String = "safe"
      returnPreCondition = MyBase.UnsafeSource()
      If returnPreCondition = "dummy" Then
        returnPreCondition = temp
      Else
        MyBase.DummyMethod(returnPreCondition)
      End If
    End Sub

    Public Sub RefReturnPreconditionConditionalWithReturnAfterIf(ByRef returnPreCondition As String)
      MyBase.DummyMethod(returnPreCondition)
      Dim temp As String = "safe"
      returnPreCondition = MyBase.UnsafeSource()
      If returnPreCondition = "dummy" Then
        returnPreCondition = temp
        MyBase.DummyMethod(returnPreCondition)
      End If
      MyBase.DummyMethod(returnPreCondition)
    End Sub
  End Class
End Namespace
