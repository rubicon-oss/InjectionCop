Imports InjectionCop.Utilities
Imports Microsoft.FxCop.Sdk
Imports NUnit.Framework
Imports System
Imports InjectionCop.IntegrationTests.VB.Parser

Namespace Utilities
  <TestFixture()>
  Public Class IntrospectionUtility_ClassTest
    <Test()>
    Public Sub IsVariable_FieldInMembebinding_IsTrue()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingField", New TypeNode() {})
      Dim expressionBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim expressionStatement As ExpressionStatement = CType(expressionBlock.Statements(1), ExpressionStatement)
      Dim sampleUnaryExpression As UnaryExpression = CType(expressionStatement.Expression, UnaryExpression)
      Dim sampleMethodCall As MethodCall = CType(sampleUnaryExpression.Operand, MethodCall)
      Dim sampleExpression As Expression = sampleMethodCall.Operands(0)
      Dim fieldIsVariable As Boolean = IntrospectionUtility.IsVariable(sampleExpression)
      Assert.That(fieldIsVariable, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsVariable_Method_IsFalse()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingField", New TypeNode() {})
      Dim expressionBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim expressionStatement As ExpressionStatement = CType(expressionBlock.Statements(1), ExpressionStatement)
      Dim sampleUnaryExpression As UnaryExpression = CType(expressionStatement.Expression, UnaryExpression)
      Dim sampleMethodCall As MethodCall = CType(sampleUnaryExpression.Operand, MethodCall)
      Dim methodIsVariable As Boolean = IntrospectionUtility.IsVariable(sampleMethodCall)
      Assert.That(methodIsVariable, [Is].[False])
    End Sub

    <Test()>
    Public Sub GetVariableName_FieldInMemberbinding_ReturnsName()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingField", New TypeNode() {})
      Dim expressionBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim expressionStatement As ExpressionStatement = CType(expressionBlock.Statements(1), ExpressionStatement)
      Dim sampleUnaryExpression As UnaryExpression = CType(expressionStatement.Expression, UnaryExpression)
      Dim sampleMethodCall As MethodCall = CType(sampleUnaryExpression.Operand, MethodCall)
      Dim sampleExpression As Expression = sampleMethodCall.Operands(0)
      Dim variableName As String = IntrospectionUtility.GetVariableName(sampleExpression)
      Assert.That(variableName, [Is].EqualTo("_field"))
    End Sub

    <Test()>
    Public Sub GetVariableName_Method_IsNull()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingField", New TypeNode() {})
      Dim expressionBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim expressionStatement As ExpressionStatement = CType(expressionBlock.Statements(1), ExpressionStatement)
      Dim sampleUnaryExpression As UnaryExpression = CType(expressionStatement.Expression, UnaryExpression)
      Dim sampleMethodCall As MethodCall = CType(sampleUnaryExpression.Operand, MethodCall)
      Dim variableName As String = IntrospectionUtility.GetVariableName(sampleMethodCall)
      Assert.That(variableName, [Is].Null)
    End Sub

    <Test()>
    Public Sub IsVariableWithOutParameter_FieldInMemberbinding_IsTrueAndReturnsFieldName()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingField", New TypeNode() {})
      Dim expressionBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim expressionStatement As ExpressionStatement = CType(expressionBlock.Statements(1), ExpressionStatement)
      Dim sampleUnaryExpression As UnaryExpression = CType(expressionStatement.Expression, UnaryExpression)
      Dim sampleMethodCall As MethodCall = CType(sampleUnaryExpression.Operand, MethodCall)
      Dim sampleExpression As Expression = sampleMethodCall.Operands(0)
      Dim variableName As String
      Dim fieldIsVariable As Boolean = IntrospectionUtility.IsVariable(sampleExpression, variableName)
      Dim correctName As Boolean = variableName = "_field"
      Assert.That(fieldIsVariable AndAlso correctName, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsVariableWithOutParameter_Method_IsFalseAndReturnsNullAsVariableName()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingField", New TypeNode() {})
      Dim expressionBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim expressionStatement As ExpressionStatement = CType(expressionBlock.Statements(1), ExpressionStatement)
      Dim sampleUnaryExpression As UnaryExpression = CType(expressionStatement.Expression, UnaryExpression)
      Dim sampleMethodCall As MethodCall = CType(sampleUnaryExpression.Operand, MethodCall)
      Dim variableName As String
      Dim methodIsVariable As Boolean = IntrospectionUtility.IsVariable(sampleMethodCall, variableName)
      Dim correctName As Boolean = variableName = Nothing
      Assert.That(Not methodIsVariable AndAlso correctName, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsField_FieldExpression_ReturnsTrue()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("FieldAssignment", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignmentStatement As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignmentStatement.Target
      Dim fieldRecognized As Boolean = IntrospectionUtility.IsField(sampleExpression)
      Assert.That(fieldRecognized, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsField_NonFieldAssignment_ReturnsFalse()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("NonFieldAssignment", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignmentStatement As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignmentStatement.Target
      Dim fieldRecognized As Boolean = IntrospectionUtility.IsField(sampleExpression)
      Assert.That(fieldRecognized, [Is].[False])
    End Sub

    <Test()>
    Public Sub GetField_FieldExpression_ReturnsContainedField()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("FieldAssignment", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignmentStatement As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignmentStatement.Target
      Dim extractedField As Field = IntrospectionUtility.GetField(sampleExpression)
      Dim boundMember As Member = (CType(sampleExpression, MemberBinding)).BoundMember
      Dim extractionCorrect As Boolean = extractedField.UniqueKey = boundMember.UniqueKey
      Assert.That(extractionCorrect, [Is].[True])
    End Sub

    <Test()>
    Public Sub GetField_NonFieldAssignment_ReturnsNull()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("NonFieldAssignment", New TypeNode() {})
      Dim assignmentBlock As Block = CType(sample.Body.Statements(0), Block)
      Dim assignmentStatement As AssignmentStatement = CType(assignmentBlock.Statements(1), AssignmentStatement)
      Dim sampleExpression As Expression = assignmentStatement.Target
      Dim extractedField As Field = IntrospectionUtility.GetField(sampleExpression)
      Dim extractionCorrect As Boolean = extractedField Is Nothing
      Assert.That(extractionCorrect, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsPropertyGetter_PropertyGetter_ReturnsTrue()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("get_AnyProperty", New TypeNode() {})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertyGetter(sample)
      Assert.That(isPropertyGetter, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsPropertyGetter_PropertySetter_ReturnsFalse()
      Dim objectTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Object)()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("set_AnyProperty", New TypeNode() {objectTypeNode})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertyGetter(sample)
      Assert.That(isPropertyGetter, [Is].[False])
    End Sub

    <Test()>
    Public Sub IsPropertyGetter_MethodNamedLikeGetter_ReturnsFalse()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("get_NonExistingProperty", New TypeNode() {})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertyGetter(sample)
      Assert.That(isPropertyGetter, [Is].[False])
    End Sub

    <Test()>
    Public Sub IsPropertyGetter_MethodWithParameterNamedLikeGetter_ReturnsFalse()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("get_NonExistingProperty", New TypeNode() {stringTypeNode})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertyGetter(sample)
      Assert.That(isPropertyGetter, [Is].[False])
    End Sub

    <Test()>
    Public Sub IsPropertyGetter_DummyMethod_ReturnsFalse()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("Dummy", New TypeNode() {stringTypeNode})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertyGetter(sample)
      Assert.That(isPropertyGetter, [Is].[False])
    End Sub

    <Test()>
    Public Sub IsPropertySetter_PropertySetter_ReturnsTrue()
      Dim objectTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of Object)()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("set_AnyProperty", New TypeNode() {objectTypeNode})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertySetter(sample)
      Assert.That(isPropertyGetter, [Is].[True])
    End Sub

    <Test()>
    Public Sub IsPropertySetter_DummyMethod_ReturnsFalse()
      Dim stringTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of String)()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("Dummy", New TypeNode() {stringTypeNode})
      Dim isPropertySetter As Boolean = IntrospectionUtility.IsPropertySetter(sample)
      Assert.That(isPropertySetter, [Is].[False])
    End Sub

    <Test()>
    Public Sub IsPropertySetter_PropertyGetter_ReturnsFalse()
      Dim sample As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("get_AnyProperty", New TypeNode() {})
      Dim isPropertyGetter As Boolean = IntrospectionUtility.IsPropertySetter(sample)
      Assert.That(isPropertyGetter, [Is].[False])
    End Sub

    <Test()>
    Public Sub GetNestedType_ExistingNestedType_ReturnsNestedType()
      Dim parent As TypeNode = IntrospectionUtility.TypeNodeFactory(Of IntrospectionUtility_ClassSample)()
      Dim nestedType As TypeNode = IntrospectionUtility.GetNestedType(parent, "NestedClass")
      Assert.That(nestedType, [Is].[Not].Null)
    End Sub

    <Test()>
    Public Sub GetNestedType_NonExistingNestedType_ReturnsNestedType()
      Dim parent As TypeNode = IntrospectionUtility.TypeNodeFactory(Of IntrospectionUtility_ClassSample)()
      Dim nestedType As TypeNode = IntrospectionUtility.GetNestedType(parent, "DoesNotExist")
      Assert.That(nestedType, [Is].Null)
    End Sub

    <Test()>
    Public Sub IsVariable_ArrayVariable_ReturnsTrue()
      Dim sampleMethod As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("ArrayVariableAndIndexer", New TypeNode() {})
      Dim sampleBlock As Block = CType(sampleMethod.Body.Statements(0), Block)
      Dim sampleAssignment As AssignmentStatement = CType(sampleBlock.Statements(1), AssignmentStatement)
      Dim sample As Expression = sampleAssignment.Target
      Assert.That(IntrospectionUtility.IsVariable(sample), [Is].[True])
    End Sub

    <Test()>
    Public Sub GetVariableName_ArrayVariable_ReturnsCorrectName()
      Dim sampleMethod As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("ArrayVariableAndIndexer", New TypeNode() {})
      Dim sampleBlock As Block = CType(sampleMethod.Body.Statements(0), Block)
      Dim sampleAssignment As AssignmentStatement = CType(sampleBlock.Statements(1), AssignmentStatement)
      Dim sample As Expression = sampleAssignment.Target
      Assert.That(IntrospectionUtility.GetVariableName(sample), [Is].EqualTo("local$2"))
    End Sub

    <Test()>
    Public Sub IsVariable_Indexer_ReturnsTrue()
      Dim sampleMethod As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("ArrayVariableAndIndexer", New TypeNode() {})
      Dim sampleBlock As Block = CType(sampleMethod.Body.Statements(0), Block)
      Dim sampleAssignment As AssignmentStatement = CType(sampleBlock.Statements(2), AssignmentStatement)
      Dim sample As Expression = sampleAssignment.Target
      Assert.That(IntrospectionUtility.IsVariable(sample), [Is].[True])
    End Sub

    <Test()>
    Public Sub GetVariableName_Indexer_ReturnsNameOfDeclaringObject()
      Dim sampleMethod As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("ArrayVariableAndIndexer", New TypeNode() {})
      Dim sampleBlock As Block = CType(sampleMethod.Body.Statements(0), Block)
      Dim sampleAssignment As AssignmentStatement = CType(sampleBlock.Statements(2), AssignmentStatement)
      Dim sample As Expression = sampleAssignment.Target
      Assert.That(IntrospectionUtility.GetVariableName(sample), [Is].EqualTo("local$2"))
    End Sub

    <Test()>
    Public Sub IsCompilerGenerated_UserGeneratedTypeNode_ReturnsFalse()
      Dim sampleTypeNode As TypeNode = IntrospectionUtility.TypeNodeFactory(Of IntrospectionUtility_ClassSample)()
      Assert.That(IntrospectionUtility.IsCompilerGenerated(sampleTypeNode), [Is].[False])
    End Sub

    <Test()>
    Public Sub IsCompilerGenerated_CompilerGeneratedTypeNode_ReturnsTrue()
      Dim sampleMethod As Method = TestHelper.GetSample(Of IntrospectionUtility_ClassSample)("UsingClosure", New TypeNode() {})
      Dim sampleBlock As Block = CType(sampleMethod.Body.Statements(0), Block)
      Dim sampleAssignment As AssignmentStatement = CType(sampleBlock.Statements(1), AssignmentStatement)
      Dim compilerGeneratedTypeNode As TypeNode = sampleAssignment.Source.Type
      Assert.That(IntrospectionUtility.IsCompilerGenerated(compilerGeneratedTypeNode), [Is].[True])
    End Sub
  End Class
End Namespace
