Imports System

Namespace Parser
	<System.AttributeUsage(System.AttributeTargets.Parameter Or System.AttributeTargets.ReturnValue)>
	Public Class NonFragmentAttribute
		Inherits System.Attribute

		Private _fragmentType As String

		Public ReadOnly Property FragmentType() As String
			Get
				Return Me._fragmentType
			End Get
		End Property

		Public Sub New()
			Me._fragmentType = "Fragment"
		End Sub

		Public Sub New(fragmentType As String)
			Me._fragmentType = fragmentType
		End Sub

		Public Shared Operator=(a As NonFragmentAttribute, b As NonFragmentAttribute) As Boolean
			Return Object.ReferenceEquals(a, b) OrElse (a IsNot Nothing AndAlso b IsNot Nothing AndAlso a._fragmentType = b._fragmentType)
		End Operator

		Public Shared Operator<>(a As NonFragmentAttribute, b As NonFragmentAttribute) As Boolean
			Return Not(a Is b)
		End Operator

    Public Overloads Function Equals(otherAttribute As NonFragmentAttribute) As Boolean
      Return Me._fragmentType = otherAttribute._fragmentType
    End Function

    Public Overrides Function Equals(objectparameter As Object) As Boolean
      Dim result As Boolean
      If objectparameter Is Nothing Then
        result = False
      Else
        Dim nonFragmentAttribute As NonFragmentAttribute = TryCast(objectparameter, NonFragmentAttribute)
        result = (Not (nonFragmentAttribute Is Nothing) AndAlso Me._fragmentType = nonFragmentAttribute._fragmentType)
      End If
      Return result
    End Function

    Public Overrides Function GetHashCode() As Integer
      Return MyBase.GetHashCode() * 397 Xor (If((Me._fragmentType IsNot Nothing), Me._fragmentType.GetHashCode(), 0))
    End Function

    Public Shared Function OfType(fragmentattribute As String) As NonFragmentAttribute
      Return New NonFragmentAttribute(fragmentattribute)
    End Function
  End Class
End Namespace
