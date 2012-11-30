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
using InjectionCop.Utilities;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Utilities
{
  [TestFixture]
  public class ArgumentUtilityTest
  {
    [Test]
    [ExpectedException (typeof (ArgumentNullException))]
    public void CheckNotNull_Nullable_Fail()
    {
      ArgumentUtility.CheckNotNull ("arg", (int?) null);
    }

    [Test]
    public void CheckNotNull_Nullable_Succeed()
    {
      int? result = ArgumentUtility.CheckNotNull ("arg", (int?) 1);
      Assert.That (result, Is.EqualTo (1));
    }

    [Test]
    public void CheckNotNull_Value_Succeed()
    {
      int result = ArgumentUtility.CheckNotNull ("arg", 1);
      Assert.That (result, Is.EqualTo (1));
    }

    [Test]
    [ExpectedException (typeof (ArgumentNullException))]
    public void CheckNotNull_Reference_Fail()
    {
      ArgumentUtility.CheckNotNull ("arg", (string) null);
    }

    [Test]
    public void CheckNotNull_Reference_Succeed()
    {
      string result = ArgumentUtility.CheckNotNull ("arg", string.Empty);
      Assert.That (result, Is.SameAs (string.Empty));
    }
    
    [Test]
    [ExpectedException (typeof (ArgumentNullException))]
    public void CheckNotNullOrEmpty_Fail_NullString ()
    {
      const string value = null;
      ArgumentUtility.CheckNotNullOrEmpty ("arg", value);
    }

    [Test]
    [ExpectedException (typeof (ArgumentEmptyException))]
    public void CheckNotNullOrEmpty_Fail_EmptyString ()
    {
      ArgumentUtility.CheckNotNullOrEmpty ("arg", "");
    }

    [Test]
    public void CheckNotNullOrEmpty_Succeed_String ()
    {
      string result = ArgumentUtility.CheckNotNullOrEmpty ("arg", "Test");
      Assert.That (result, Is.EqualTo ("Test"));
    }
  }
}