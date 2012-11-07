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
    public void ReferenceEquals_SameObject_True ()
    {
      FragmentAttribute a = new FragmentAttribute("fragment");
      FragmentAttribute b = a;
      Assert.That (a == b, Is.True);
    }

    [Test]
    public void ReferenceEquals_OneNull_False ()
    {
      FragmentAttribute a = new FragmentAttribute("fragment");
      FragmentAttribute b = null;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
      Assert.That (a == b, Is.False);
// ReSharper restore ConditionIsAlwaysTrueOrFalse
    }

    [Test]
    public void ReferenceEquals_BothNull_False ()
    {
      FragmentAttribute a = null;
      FragmentAttribute b = null;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
      Assert.That (a == b, Is.True);
// ReSharper restore ConditionIsAlwaysTrueOrFalse
    }

    [Test]
    public void ReferenceEquals_SameFragmentType_True ()
    {
      FragmentAttribute a = new FragmentAttribute("fragment");
      FragmentAttribute b = new FragmentAttribute("fragment");
      Assert.That (a == b, Is.True);
    }

    [Test]
    public void Equals_EqualFragment_True()
    {
      FragmentAttribute a = new FragmentAttribute ("fragmenttype");
      FragmentAttribute b = new FragmentAttribute ("fragmenttype");
      Assert.That (a.Equals (b));
    }

    [Test]
    public void Equals_UnequalFragment_False()
    {
      FragmentAttribute a = new FragmentAttribute("a");
      FragmentAttribute b = new FragmentAttribute("b");
      Assert.That (a.Equals (b), Is.False);
    }

    [Test]
    public void Equals_EqualObject_True()
    {
      FragmentAttribute a = new FragmentAttribute("fragmenttype");
      object b = new FragmentAttribute("fragmenttype");
      Assert.That (a.Equals (b), Is.True);
    }

    [Test]
    public void Equals_UnequalObject_False()
    {
      FragmentAttribute a = new FragmentAttribute("a");
      Object b = new FragmentAttribute("b");
      Assert.That (a.Equals (b), Is.False);
    }

    [Test]
    public void OfType_EqualFragments_True()
    {
      FragmentAttribute fragmentAttribute = new FragmentAttribute ("fragmentType");
      Assert.That(FragmentAttribute.OfType ("fragmentType").Equals (fragmentAttribute), Is.True);
    }

    [Test]
    public void OfType_UnequalFragments_False()
    {
      FragmentAttribute fragmentAttribute = new FragmentAttribute ("fragmentType");
      Assert.That(FragmentAttribute.OfType ("otherType").Equals (fragmentAttribute), Is.False);
    }
  }
}
