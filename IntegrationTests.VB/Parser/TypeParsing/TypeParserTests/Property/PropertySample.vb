Imports InjectionCop.Fragment
Imports System

Namespace Parser.TypeParsing.TypeParserTests.[Property]
	Public Class PropertySample
		Inherits ParserSampleBase

		Public Property SafePropertyVerboseAnnotation() As String

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
