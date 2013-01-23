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
using NUnit.Framework;

namespace InjectionCop.UnitTests.Parser
{
  [TestFixture]
  public class SymbolTableTest
  {
    private SymbolTable _symbolTable;


    [SetUp]
    public void SetUp ()
    {
      _symbolTable = new SymbolTable(new IDbCommandBlacklistManagerStub());
    }

    [Test]
    public void Clone_ReturnsDeepCopy_True ()
    {
      _symbolTable.MakeSafe ("key", Fragment.CreateNamed( "FragmentType"));
      ISymbolTable clone = _symbolTable.Copy();
      clone.MakeUnsafe ("key");
      ISymbolTable result = _symbolTable.Copy();
      
      Assert.That (result.IsAssignableTo("key", Fragment.CreateNamed( "FragmentType")), Is.True);
    }

    [Test]
    public void GetSafenessMap_ExistingEntry_ReturnsEntry ()
    {
      _symbolTable.MakeSafe ("key", Fragment.CreateNamed ("FragmentType"));
      var fragmentType = _symbolTable.GetFragmentType ("key");

      Assert.That (fragmentType, Is.EqualTo (Fragment.CreateNamed ("FragmentType")));
    }

    [Test]
    public void GetSafenessMap_NonExistingEntry_ReturnsEmptyFragment ()
    {
      Assert.That(_symbolTable.GetFragmentType ("key"), Is.EqualTo(Fragment.CreateEmpty()));
    }
  }
}
