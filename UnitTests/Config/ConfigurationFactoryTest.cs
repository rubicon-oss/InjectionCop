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
using System.IO;
using InjectionCop.Config;
using InjectionCop.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace InjectionCop.UnitTests.Config
{
  public class ConfigurationFactoryTest
  {
    [Test]
    public void CreateFrom_UsesConfigurationFileLocator ()
    {
      var config1Location = Path.Combine (Path.GetDirectoryName (GetType().Assembly.Location), "config.injectioncop");
      var config2Location = Path.Combine (Path.GetDirectoryName (typeof (ConfigurationFactory).Assembly.Location), "config.injectioncop");

      var emptyConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
</Blacklist>";
      var typeNode = IntrospectionUtility.TypeNodeFactory<ConfigurationFileLocatorTest>();
      var configurationFileLocatorMock = MockRepository.GenerateStrictMock<IConfigurationFileLocator>();

      configurationFileLocatorMock.Expect (mock => mock.GetFilesFromCurrentAssembly()).Return (new[] { config1Location });
      configurationFileLocatorMock.Expect (mock => mock.GetFilesFromParsedType (typeNode)).Return (new[] { config2Location });
      configurationFileLocatorMock.Replay();

      try
      {
        File.WriteAllText (config1Location, emptyConfig);
        File.WriteAllText (config2Location, emptyConfig);

        ConfigurationFactory.CreateFrom (typeNode, configurationFileLocatorMock);
        configurationFileLocatorMock.VerifyAllExpectations();
      }
      finally
      {
        File.Delete (config1Location);
        File.Delete (config2Location);
      }
    }

    [Test]
    public void CreateFrom_CreatesLayeredConfiguration ()
    {
      var config1Location = Path.Combine (Path.GetDirectoryName (GetType().Assembly.Location), "config.injectioncop");
      var config2Location = Path.Combine (Path.GetDirectoryName (typeof (ConfigurationFactory).Assembly.Location), "config.injectioncop");

      var emptyConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Blacklist xmlns=""http://injectioncop.codeplex.com/"">
</Blacklist>";
      var typeNode = IntrospectionUtility.TypeNodeFactory<ConfigurationFileLocatorTest>();
      var configurationFileLocatorStub = MockRepository.GenerateStub<IConfigurationFileLocator>();

      configurationFileLocatorStub.Stub (mock => mock.GetFilesFromCurrentAssembly()).Return (new[] { config1Location });
      configurationFileLocatorStub.Stub (mock => mock.GetFilesFromParsedType (typeNode)).Return (new[] { config2Location });

      try
      {
        File.WriteAllText (config1Location, emptyConfig);
        File.WriteAllText (config2Location, emptyConfig);

        var configuration = ConfigurationFactory.CreateFrom (typeNode, configurationFileLocatorStub);

        Assert.That (configuration, Is.InstanceOf<LayeredConfigurationAdapter>());

        var layeredConfigurationAdapter = (LayeredConfigurationAdapter) configuration;
        Assert.That (layeredConfigurationAdapter.Configurations.Length, Is.EqualTo (2));
      }
      finally
      {
        File.Delete (config1Location);
        File.Delete (config2Location);
      }
    }

    [Test]
    public void CreateFrom_NoConfigurationFilesFound ()
    {
      var typeNode = IntrospectionUtility.TypeNodeFactory<ConfigurationFileLocatorTest>();
      var configurationFileLocatorStub = MockRepository.GenerateStub<IConfigurationFileLocator>();

      configurationFileLocatorStub.Stub (mock => mock.GetFilesFromCurrentAssembly()).Return (new string[0]);
      configurationFileLocatorStub.Stub (mock => mock.GetFilesFromParsedType (typeNode)).Return (new string[0]);

      var configuration = ConfigurationFactory.CreateFrom (typeNode, configurationFileLocatorStub);

      Assert.That (configuration, Is.InstanceOf<LayeredConfigurationAdapter>());

      var layeredConfigurationAdapter = (LayeredConfigurationAdapter) configuration;
      Assert.That (layeredConfigurationAdapter.Configurations, Is.Empty);
    }
  }
}