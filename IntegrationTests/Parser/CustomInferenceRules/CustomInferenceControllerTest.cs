// Copyright 2013 rubicon informationstechnologie gmbh
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.CustomInferenceRules;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.IntegrationTests.Parser.CustomInferenceRules
{
  [TestFixture]
  public class CustomInferenceControllerTest
  {
    private CustomInferenceController _customInferenceController;
    private ISymbolTable _symbolTable;

    [SetUp]
    public void SetUp ()
    {
      MockRepository mocks = new MockRepository();
      IBlacklistManager blacklistManager = mocks.Stub<IBlacklistManager>();
      _customInferenceController = new CustomInferenceController();
      _symbolTable = new SymbolTable (blacklistManager);
    }

    [Test]
    public void Covers_CoveredMethod_ReturnsTrue ()
    {
      TypeNode stringTypeNode = IntrospectionUtility.TypeNodeFactory<string>();
      Method sample = IntrospectionUtility.MethodFactory<string> ("Concat", stringTypeNode, stringTypeNode);

      Assert.That (_customInferenceController.Covers (sample), Is.True);
    }

    [Test]
    public void Covers_UncoveredMethod_ReturnsFalse ()
    {
      Method sample = IntrospectionUtility.MethodFactory<string> ("ToString");
      
      Assert.That (_customInferenceController.Covers (sample), Is.False);
    }

    [Test]
    public void InferFragmentType_SupportetCall_ReturnsFragment ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<CustomInferenceControllerSample> ("SupportetCall");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      AssignmentStatement sampleAssignment = (AssignmentStatement) sampleBlock.Statements[1];
      MethodCall sampleMethodCall = (MethodCall) sampleAssignment.Source;
      Fragment returnedFragment = _customInferenceController.InferFragmentType (sampleMethodCall, _symbolTable);

      Assert.That (returnedFragment, Is.EqualTo (Fragment.CreateNamed ("SqlFragment")));
    }

    [Test]
    public void InferFragmentType_UnsupportetCall_ReturnsFragment ()
    {
      Method sampleMethod = IntrospectionUtility.MethodFactory<CustomInferenceControllerSample> ("UnsupportetCall");
      Block sampleBlock = (Block) sampleMethod.Body.Statements[0];
      ExpressionStatement sampleExpression = (ExpressionStatement) sampleBlock.Statements[1];
      MethodCall sampleMethodCall = (MethodCall) sampleExpression.Expression;
      Fragment returnedFragment = _customInferenceController.InferFragmentType (sampleMethodCall, _symbolTable);

      Assert.That (returnedFragment, Is.EqualTo (Fragment.CreateEmpty()));
    }
  }
}
