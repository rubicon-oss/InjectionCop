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

Namespace Parser.TypeParsing.TypeParserTests.Inheritance.Constructor
	Friend Class InheritanceSampleConstructor
		Inherits InheritanceSampleBase

		Public Sub New()
      MyBase.New("safe", "safe")
		End Sub

		Public Sub New(dummy As String)
      MyBase.New(InheritanceSampleBase.UnsafeInheritanceFragmentSource(), "safe")
			Me._initialNonFragmentField = dummy
		End Sub

		Public Sub New(dummy1 As String, dummy2 As String)
			MyBase.New(dummy1)
			Me._initialNonFragmentField = dummy2
		End Sub
	End Class
End Namespace
