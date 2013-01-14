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
Imports System

Namespace Parser.TypeParsing.TypeParserTests.Attribute
	<System.AttributeUsage(System.AttributeTargets.Parameter Or System.AttributeTargets.ReturnValue)>
	Public Class SampleAttribute
		Inherits System.Attribute

		Private _fragmentType As String

		Public ReadOnly Property FragmentType() As String
			Get
				Return Me._fragmentType
			End Get
		End Property

		Public Sub New(fragmentType As String)
			Me._fragmentType = fragmentType
		End Sub
	End Class
End Namespace
