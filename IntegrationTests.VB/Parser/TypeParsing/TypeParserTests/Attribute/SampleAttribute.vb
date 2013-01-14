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
