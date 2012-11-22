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
using System.Collections.Generic;
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;
using InjectionCop.IntegrationTests.Parser;

namespace InjectionCop.UnitTests.Parser
{
  [TestFixture]
  public class SymbolTableTest
  {
    private SymbolTable _symbolTable;

    [SetUp]
    public void SetUp ()
    {
      _symbolTable = new SymbolTable(new IDbCommandBlackTypesStub());
    }

    [Test]
    public void Clone_ReturnsDeepCopy_True ()
    {
      _symbolTable.MakeSafe ("key", "FragmentType");
      SymbolTable clone = _symbolTable.Clone();
      clone.MakeUnsafe ("key");
      SymbolTable result = _symbolTable.Clone();
      
      Assert.That (result.IsSafe("key", "FragmentType"), Is.True);
    }

    [Test]
    public void GetSafenessMap_ExistingEntry_ReturnsEntry ()
    {
      _symbolTable.MakeSafe ("key", "FragmentType");
      bool safeness =_symbolTable.GetContextMap ("key")["FragmentType"];

      Assert.That (safeness, Is.True);
    }

    [Test]
    [ExpectedException(typeof(InjectionCopException), ExpectedMessage = "Given Symbolname not found in Symboltable")]
    public void GetSafenessMap_NonExistingEntry_ThrowsException ()
    {
      _symbolTable.GetContextMap ("key");
    }
  }
}
