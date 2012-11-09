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
using InjectionCop.IntegrationTests.Parser;
using InjectionCop.Parser;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Parser
{
  [TestFixture]
  public class BasicBlockTest
  {
    /*
    [Test]
    public void PreconditionSafeSymbols_UsesShallowCopy_False ()
    {
      BasicBlock basicBlock = new BasicBlock();
      List<string> safeSymbols = new List<string>();
      safeSymbols.Add ("safevariablename");
      basicBlock.PreConditionSafeSymbols = safeSymbols;
      List<string> precondition = basicBlock.PreConditionSafeSymbols;
      precondition.Add ("additional");
      List<string> result = basicBlock.PreConditionSafeSymbols;

      Assert.That (result.Contains ("additional"), Is.False);
    }

    [Test]
    public void PostconditionSymbolTable_UsesDeepcopy_True()
    {
      SymbolTable symbolTable = new SymbolTable(new IDbCommandBlackTypesStub());
      symbolTable.SetSafeness ("key", true);
      BasicBlock basicBlock = new BasicBlock();
      basicBlock.PostConditionSymbolTable = symbolTable;
      symbolTable.SetSafeness ("key", false);

      Assert.That (basicBlock.PostConditionSymbolTable.IsSafe("key"), Is.True);
    }*/
  }
}
