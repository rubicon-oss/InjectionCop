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

Namespace Parser.TypeParsing.TypeParserTests.[Property]
	Public Class PropertySample
		Inherits ParserSampleBase

    Private _safePropertyVerboseAnnotation As String
    Public Property SafePropertyVerboseAnnotation() As <Fragment("SqlFragment")> String
      Get
        Return _safePropertyVerboseAnnotation
      End Get
      Set(<Fragment("SqlFragment")> value As String)
        _safePropertyVerboseAnnotation = value
      End Set
    End Property

		<Fragment("SqlFragment")>
		Public Property SafeProperty() As String

		Public Property UnsafeProp() As String

		Public Sub CallWithUnsafeProperty()
			MyBase.RequiresSqlFragment(Me.UnsafeProp)
		End Sub

		Public Sub CallWithSafePropertyVerboseAnnotation()
			MyBase.RequiresSqlFragment(Me.SafePropertyVerboseAnnotation)
		End Sub

		Public Sub SetSafePropertyVerboseAnnotationWithSafeValue()
			Me.SafePropertyVerboseAnnotation = "safe"
		End Sub

		Public Sub SetSafePropertyVerboseAnnotationWithUnsafeValue()
			Me.SafePropertyVerboseAnnotation = MyBase.UnsafeSource()
		End Sub

		Public Sub CallWithSafeProperty()
			MyBase.RequiresSqlFragment(Me.SafeProperty)
		End Sub

		Public Sub SetSafePropertyWithSafeValue()
			Me.SafeProperty = "safe"
		End Sub

		Public Sub SetSafePropertyWithUnsafeValue()
			Me.SafeProperty = MyBase.UnsafeSource()
		End Sub
	End Class
End Namespace
