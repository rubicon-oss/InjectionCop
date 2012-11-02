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
using InjectionCop.Attributes;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Attributes
{
  [TestFixture]
  public class FragmentAttributeTest
  {
    [Test]
    public void Eq_SameObject_True ()
    {
      FragmentAttribute a = new FragmentAttribute("fragment");
      FragmentAttribute b = a;
      Assert.That (a == b, Is.True);
    }

    [Test]
    public void Eq_OneNull_False ()
    {
      FragmentAttribute a = new FragmentAttribute("fragment");
      FragmentAttribute b = null;
      Assert.That (a == b, Is.False);
    }

    [Test]
    public void Eq_BothNull_False ()
    {
      FragmentAttribute a = null;
      FragmentAttribute b = null;
      Assert.That (a == b, Is.True);
    }

    [Test]
    public void Eq_SameFragmentType_True ()
    {
      FragmentAttribute a = new FragmentAttribute("fragment");
      FragmentAttribute b = new FragmentAttribute("fragment");
      Assert.That (a == b, Is.True);
    }
  }
}
