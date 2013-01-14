Imports System

Namespace Utilities
  Public Class ClassWithMethodReturningFragment
    Implements InterfaceWithReturnFragment

    Public Function MethodWithReturnFragment() As Integer Implements InterfaceWithReturnFragment.MethodWithReturnFragment
      Return 3
    End Function
  End Class
End Namespace
