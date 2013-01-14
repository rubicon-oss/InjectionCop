Imports System

Namespace Utilities
  Public Class ClassWithExplicitlyDeclaredMethodReturningFragment
    Implements InterfaceWithReturnFragment

    Function MethodWithReturnFragment() As Integer Implements InterfaceWithReturnFragment.MethodWithReturnFragment
      Return 3
    End Function
  End Class
End Namespace
