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

namespace InjectionCop.IntegrationTests.Parser.MethodParsing
{
  [TestFixture]
  public class InitialSymbolTableBuilderTest
  {
    private IBlacklistManager _blacklistManager;

    [SetUp]
    public void SetUp()
    {
      _blacklistManager = new IDbCommandBlacklistManagerStub();
    }

    [Test]
    public void GetResult_ParameterizedMethod_ReturnsSymbolTableContainingFragmentField ()
    {
      TypeNode floatType = IntrospectionUtility.TypeNodeFactory<float>();
      TypeNode objectType = IntrospectionUtility.TypeNodeFactory<object>();
      Method sampleMethod = TestHelper.GetSample<InitialSymbolTableBuilderSample> ("ParameterizedMethod", floatType, objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder (sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();
      bool fieldIsCorrectFragment = resultSymbolTable.IsFragment("_fragmentField", "FieldType");
      Assert.That (fieldIsCorrectFragment, Is.True);
    }

    [Test]
    public void GetResult_ParameterizedMethod_ReturnsSymbolTableContainingNonFragmentField ()
    {
      TypeNode floatType = IntrospectionUtility.TypeNodeFactory<float>();
      TypeNode objectType = IntrospectionUtility.TypeNodeFactory<object>();
      Method sampleMethod = TestHelper.GetSample<InitialSymbolTableBuilderSample> ("ParameterizedMethod", floatType, objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder (sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();
      bool fieldIsCorrectFragment = resultSymbolTable.IsFragment("_nonFragmentField", SymbolTable.EMPTY_FRAGMENT);
      Assert.That (fieldIsCorrectFragment, Is.True);
    }

    [Test]
    public void GetResult_ParameterizedMethod_ReturnsSymbolTableContainingSqlFragmentField ()
    {
      TypeNode floatType = IntrospectionUtility.TypeNodeFactory<float>();
      TypeNode objectType = IntrospectionUtility.TypeNodeFactory<object>();
      Method sampleMethod = TestHelper.GetSample<InitialSymbolTableBuilderSample> ("ParameterizedMethod", floatType, objectType);
      InitialSymbolTableBuilder initialSymbolTableBuilder = new InitialSymbolTableBuilder (sampleMethod, _blacklistManager);
      ISymbolTable resultSymbolTable = initialSymbolTableBuilder.GetResult();
      bool fieldIsCorrectFragment = resultSymbolTable.IsFragment("_sqlFragmentField", "SqlFragment");
      Assert.That (fieldIsCorrectFragment, Is.True);
    }
  }
}
