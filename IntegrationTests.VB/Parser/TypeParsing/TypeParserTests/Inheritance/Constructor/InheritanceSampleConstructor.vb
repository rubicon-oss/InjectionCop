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
