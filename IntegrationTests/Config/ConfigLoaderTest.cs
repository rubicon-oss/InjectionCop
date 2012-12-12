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
using InjectionCop.Config;
using NUnit.Framework;

namespace InjectionCop.IntegrationTests.Config
{
  [TestFixture]
  public class ConfigLoaderTest
  {
    [Test]
    public void LoadBlackList_ListsXmlDefinedBlacklistedPrototypes ()
    {
      IBlacklistManager blacklist = ConfigLoader.LoadBlacklist();
      IList<string> parameters = new List<string> { "System.String" };
      string[] parameterFragmentTypes = blacklist.GetFragmentTypes("namespace.ExternallyDefined", "FragmentDefinedInXml", parameters);

      Assert.That (parameterFragmentTypes != null, Is.True);
    }

    [Test]
    public void LoadBlackList_ReturnsFragmentXmlDefinedBlacklistedPrototypes ()
    {
      IBlacklistManager blacklist = ConfigLoader.LoadBlacklist();
      IList<string> parameters = new List<string> { "System.String" };
      string[] commandTextFragmentTypes = blacklist.GetFragmentTypes("namespace.ExternallyDefined", "FragmentDefinedInXml", parameters);

      Assert.That (commandTextFragmentTypes[0], Is.EqualTo("external"));
    }

    [Test]
    public void LoadBlackList_UnknownPrototypeNotListed ()
    {
      IBlacklistManager blacklist = ConfigLoader.LoadBlacklist();
      IList<string> parameters = new List<string> ();
      string[] parameterFragmentTypes = blacklist.GetFragmentTypes("namespace.NotExistingType", "Dummy", parameters);

      Assert.That (parameterFragmentTypes, Is.Null);
    }

    [Test]
    public void LoadBlackList_KnownPrototypeNotAddedToXmlNotListed ()
    {
      IBlacklistManager blacklist = ConfigLoader.LoadBlacklist();
      IList<string> parameters = new List<string> ();
      string testNamespace = "InjectionCop.IntegrationTests.Config";
      string testMethodName = "LoadBlackList_KnownPrototypeNotAddedToXmlNotListed";
      string[] parameterFragmentTypes = blacklist.GetFragmentTypes(testNamespace, testMethodName, parameters);

      Assert.That (parameterFragmentTypes, Is.Null);
    }
  }
}
