Imports System
Imports System.Globalization

Namespace Parser.TypeParsing.TypeParserTests.TryCatchFinally
	Public Class TryCatchFinallySample
		Inherits ParserSampleBase

		Public Sub SafeCallInsideTry()
			Try
				MyBase.RequiresSqlFragment("safe")
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub UnsafeCallInsideTry()
			Try
				MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub SafeCallInsideCatch()
			Try
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			Catch ex2 As System.Exception
				MyBase.DummyMethod(ex2.Message)
				MyBase.RequiresSqlFragment("safe")
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub UnsafeCallInsideCatch()
			Try
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			Catch ex2 As System.Exception
				MyBase.RequiresSqlFragment(ex2.Message)
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub SafeCallInsideFinally()
			Try
				MyBase.RequiresSqlFragment("safe")
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			Finally
				MyBase.RequiresSqlFragment("safe")
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub UnsafeCallInsideFinally()
			Try
				MyBase.RequiresSqlFragment("safe")
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			Finally
				MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub UnsafeCallNestedTry()
			Try
				Me.ThrowsException(0)
				Try
					Me.ThrowsException(0)
					MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
				Catch ex As System.ArgumentNullException
					MyBase.DummyMethod(ex.Message)
				End Try
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub UnsafeCallNestedCatch()
			Try
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
				Try
					Me.ThrowsException(0)
					MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
				Catch e As System.ArgumentNullException
					MyBase.DummyMethod(e.Message)
				End Try
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Public Sub UnsafeCallNestedFinally()
			Try
				Me.ThrowsException(0)
			Catch ex As System.ArgumentNullException
				MyBase.DummyMethod(ex.Message)
			Finally
				Try
					Me.ThrowsException(0)
					MyBase.RequiresSqlFragment(MyBase.UnsafeSource())
				Catch e As System.ArgumentNullException
					MyBase.DummyMethod(e.Message)
				End Try
			End Try
			MyBase.DummyMethod("dummy")
		End Sub

		Private Sub ThrowsException(parameter As Integer)
			If parameter = 1 Then
				Throw New System.ArgumentNullException()
			End If
			MyBase.DummyMethod(parameter.ToString(System.Globalization.CultureInfo.InvariantCulture))
		End Sub
	End Class
End Namespace
