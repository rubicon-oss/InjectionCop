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

Namespace Parser.TypeParsing.FragmentUsageTests
  Public Class DerivedClassWithInvalidFragmentUsage
    Inherits BaseClass

    Public Overrides Sub Foo(<Fragment("InvalidFragmentType")> parameter1 As Integer, parameter2 As String)
    End Sub

    Public Overrides Sub MethodWithFragmentOnSecondParameter(parameter1 As Integer, <Fragment("InvalidFragmentTypeOnSecondParameter")> parameter2 As Integer)
    End Sub
  End Class
End Namespace
