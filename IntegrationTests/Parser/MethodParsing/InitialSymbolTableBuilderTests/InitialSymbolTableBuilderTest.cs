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
using InjectionCop.Config;
using InjectionCop.Parser;
using InjectionCop.Parser.MethodParsing;
using InjectionCop.Utilities;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using InjectionCop.IntegrationTests.Parser.MethodParsing;

namespace InjectionCop.IntegrationTests.Parser.MethodParsing.InitialSymbolTableBuilderTests
{
  [TestFixture]
  public class InitialSymbolTableBuilderTest
  {
    private IBlacklistManager _blacklistManager;
    private TypeNode _floatType;
    private TypeNode _objectType;

    [SetUp]
    public void SetUp()
    {
      _blacklistManager = new IDbCommandBlacklistManagerStub();
      _floatType = IntrospectionUtility.TypeNodeFactory<float>();
      _objectType = IntrospectionUtility.TypeNodeFactory<object>();
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTable_FragmentFieldIsListedAsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder(sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_fragmentField"), Is.EqualTo(SymbolTable.EMPTY_FRAGMENT));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTable_NonFragmentFieldIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder(sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("_nonFragmentField"), Is.EqualTo(SymbolTable.EMPTY_FRAGMENT));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTable_UnknownSymbolIsListedAsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder(sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("unknownSymbol"), Is.EqualTo(SymbolTable.EMPTY_FRAGMENT));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTable_NonFragmentParameterIsEmptyFragment()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder(sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("nonFragmentParameter"), Is.EqualTo(SymbolTable.EMPTY_FRAGMENT));
    }

    [Test]
    public void GetResult_ParameterizedMethodSymbolTable_FragmentParameterIsOfCorrectFragmentType()
    {
      Method sampleMethod = TestHelper.GetSample<SymbolTableBuilderSample>("ParameterizedMethod", _floatType, _objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder(sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();

      Assert.That(resultSymbolTable.GetFragmentType("fragmentParameter"), Is.EqualTo(Fragment.CreateNamed("FragmentType")));
    }
  }
}
