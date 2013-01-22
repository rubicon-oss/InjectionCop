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
Imports InjectionCop.Attributes
Imports System

Namespace Parser.TypeParsing.SymbolTableTests.InferFragment
  Friend Class InferFragmentSample
    Inherits ParserSampleBase

    <Fragment("SampleFragment")>
    Private _fragmentField As String = "dummy"

    Private _nonFragmentField As String = "dummy"

    <Fragment("PropertyFragmentType")>
    Public Property PropertyWithFragment() As Object

    Public Property PropertyWithoutFragment() As Integer

    Public Function AssignmentWithLiteral() As Integer
      Return 3
    End Function

    Public Function AssignmentWithLocal() As String
      Dim x As String = "safe"
      MyBase.DummyMethod(x)
      Return x
    End Function

    Public Function AssignmentWithParameter(parameter As Integer) As Integer
      Return parameter
    End Function

    Public Function AssignmentWithSafeMethodCall() As String
      Return MyBase.SafeSource()
    End Function

    Public Function AssignmentWithUnsafeMethodCall() As String
      Return MyBase.UnsafeSource()
    End Function

    Public Function AssignmentWithFragmentField() As String
      Return Me._fragmentField
    End Function

    Public Function AssignmentWithNonFragmentField() As String
      Return Me._nonFragmentField
    End Function

    Public Function AssignmentWithFragmentProperty() As Object
      Return Me.PropertyWithFragment
    End Function
  End Class
End Namespace
