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

Namespace Parser.TypeParsing.TypeParserTests.[Return]
  Friend Class ReturnSample
    Inherits ParserSampleBase

    <Fragment("DummyType")>
    Private _dummyTypeFragment As String = "safe"

    Public Function ReturnFragmentMismatch() As <Fragment("DummyType")>
    String
      Return MyBase.UnsafeSource()
    End Function

    Public Function NoReturnAnnotation() As String
      Return MyBase.UnsafeSource()
    End Function

    Public Function DeclarationWithReturn() As <Fragment("ReturnFragmentType")>
    Integer
      Return 3
    End Function

    Public Function ReturnsDummyType() As <Fragment("DummyType")>
    String
      Return Me._dummyTypeFragment
    End Function

    Public Function ReturnsFieldWithWrongType() As <Fragment("DummyType")>
    String
      Return Me._fragmentField
    End Function
  End Class
End Namespace
