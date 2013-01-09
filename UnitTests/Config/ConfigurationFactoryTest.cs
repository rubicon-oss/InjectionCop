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