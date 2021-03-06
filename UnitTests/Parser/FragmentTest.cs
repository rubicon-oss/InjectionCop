﻿// Copyright 2013 rubicon informationstechnologie gmbh
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
using InjectionCop.Parser;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Parser
{
  [TestFixture]
  public class FragmentTest
  {
    [Test]
    public void UndefinedFragmentBehavesLikeEmptyFragment ()
    {
      Assert.That (Fragment.CreateUndefined() == Fragment.CreateEmpty(), Is.True);
    }

    [Test]
    public void UndefinedFragmentIsNoLiteral ()
    {
      Assert.That (Fragment.CreateUndefined() == Fragment.CreateLiteral(), Is.False);
    }

    [Test]
    public void UndefinedFragmentIsNotNamed ()
    {
      Assert.That (Fragment.CreateUndefined() == Fragment.CreateNamed("dummy"), Is.False);
    }

    [Test]
    public void UndefinedFragmentIsUndefined ()
    {
      Assert.That (Fragment.CreateUndefined().Undefined, Is.True);
    }

    [Test]
    public void DefinedFragmentIsNotUndefined ()
    {
      Assert.That (Fragment.CreateLiteral().Undefined, Is.False);
    }
  }
}
