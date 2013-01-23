// Copyright 2012 rubicon informationstechnologie gmbh
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
using NUnit.Framework;
using InjectionCop.Utilities;
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.MethodParsing;
using Microsoft.FxCop.Sdk;
using InjectionCop.IntegrationTests.Parser.MethodParsing;

namespace InjectionCop.IntegrationTests.Parser.MethodParsing.EmbeddedInitialSymbolTableBuilderTest
{
  [TestFixture]
  public class EmbeddedInitialSymbolTableBuilderTest
  {
    private IBlacklistManager _blacklistManager;
    private ISymbolTable _environment;
    private TypeNode _floatType;
    private TypeNode _objectType;

    [SetUp]
    public void SetUp()
    {
      _blacklistManager = new IDbCommandBlacklistManagerStub();
      _environment = new SymbolTable(_blacklistManager);
      _floatType = IntrospectionUtility.TypeNodeFactory<float>();
      _objectType = IntrospectionUtility.TypeNodeFactory<object>();
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithEmptyEnvironment_FragmentFieldIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithOverlappingEnvironment_FragmentFieldIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      _environment.MakeSafe("_fragmentField", Fragment.CreateNamed("ThisShouldBeIgnored"));
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_FragmentFieldIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      _environment.MakeSafe("_nonFragmentField", Fragment.CreateNamed("ThisShouldBeIgnored"));
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithEmptyEnvironment_NonFragmentFieldIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_nonFragmentField"), Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_EnvironmentSymbolIsPropagated()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      _environment.MakeSafe("environmentSymbol",Fragment.CreateNamed( "DummyType"));
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("environmentSymbol"), Is.EqualTo(Fragment.CreateNamed("DummyType")));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithOverlappingNonFragmentField_EnvironmentSymbolIsPropagated()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      _environment.MakeSafe("_nonFragmentField", Fragment.CreateNamed("OverlappingType"));
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_nonFragmentField"), Is.EqualTo(Fragment.CreateNamed("OverlappingType")));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithEmptyEnvironment_UnknownSymbolIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("unknownSymbol"), Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_NonFragmentParameterIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("nonFragmentParameter"), Is.EqualTo(Fragment.CreateEmpty()));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithNonOverlappingEnvironment_FragmentParameterIsOfCorrectFragmentType()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("fragmentParameter"), Is.EqualTo(Fragment.CreateNamed("FragmentType")));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTableBuiltWithParameterOverlappingEnvironment_FragmentParameterIsOfPropagatedFragmentType()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      _environment.MakeSafe("fragmentParameter", Fragment.CreateNamed("OverlappingType"));
      EmbeddedInitialSymbolTableBuilder embeddedInitialSymbolTableBuilder = new EmbeddedInitialSymbolTableBuilder(sampleMethod, _blacklistManager, _environment);
      ISymbolTable resultSymbolTable = embeddedInitialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("fragmentParameter"), Is.EqualTo(Fragment.CreateNamed("OverlappingType")));
    }
  }
}
