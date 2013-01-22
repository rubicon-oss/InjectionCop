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
using InjectionCop.Parser.BlockParsing.PreCondition;
using NUnit.Framework;
using InjectionCop.Parser.ProblemPipe;
using InjectionCop.Parser;
using InjectionCop.Parser.BlockParsing;
using InjectionCop.Config;
using Microsoft.FxCop.Sdk;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Parser.BlockParsing
{
  [TestFixture]
  public class AssignabilityPreConditionTest
  {
    private MockRepository _mocks;

    [SetUp]
    public void SetUp()
    {
      _mocks = new MockRepository();
    }

    [Test]
    public void IsViolated_ViolatingContext_ChangesProblemMetadatasGivenType()
    {
      var expectedFragment = Fragment.CreateNamed( "expectedFragment");
      var unexpectedFragment = Fragment.CreateNamed( "unexpectedFragment");
      ProblemMetadata problemMetaData = new ProblemMetadata(0, new SourceContext(), expectedFragment, Fragment.CreateNamed("dummy"));
      IBlacklistManager blackListManager = _mocks.Stub<IBlacklistManager>();
      AssignabilityPreCondition preCondition = new AssignabilityPreCondition("testSymbol", expectedFragment, problemMetaData);
      SymbolTable context = new SymbolTable(blackListManager);
      context.MakeSafe("testSymbol", unexpectedFragment);

      preCondition.IsViolated(context);

      Assert.That(problemMetaData.GivenFragment, Is.EqualTo(unexpectedFragment));
    }
  }
}
