using System;
using System.IO;
using InjectionCop.Config;
using InjectionCop.Utilities;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Config
{
  [TestFixture]
  public class ConfigurationFileLocatorTest
  {

    [Test]
    public void GetFilesFromParsedType ()
    {
      var orignialAssemblyLocation = Path.GetDirectoryName (new Uri(GetType().Assembly.CodeBase).AbsolutePath);
      var sampleConfigLocation = Path.Combine (orignialAssemblyLocation, "Config\\SampleConfig.xml");
      var expectedConfigLocation = Path.Combine (Path.GetDirectoryName(GetType().Assembly.Location), "config.injectioncop");
      var typeNode = IntrospectionUtility.TypeNodeFactory<ConfigurationFileLocatorTest>();

      try
      {
        File.Copy (sampleConfigLocation, expectedConfigLocation);

        var configurationFileLocator = new ConfigurationFileLocator();
        var files = configurationFileLocator.GetFilesFromParsedType (typeNode);

        Assert.That (files, Contains.Item (Path.GetFullPath (expectedConfigLocation)));
      }
      finally
      {
        File.Delete (expectedConfigLocation);
      }
    }

    [Test]
    public void GetFilesFromParsedType_NoConfigFilesFound ()
    {
      var typeNodeFactory = IntrospectionUtility.TypeNodeFactory<ConfigurationFileLocatorTest>();

      var configurationFileLocator = new ConfigurationFileLocator();
      var files = configurationFileLocator.GetFilesFromParsedType (typeNodeFactory);

      Assert.That (files, Is.Empty);
    }

    [Test]
    public void GetFilesFromCurrentAssembly ()
    {
      var configurationFileLocator = new ConfigurationFileLocator();
   
      var orignialAssemblyLocation = Path.GetDirectoryName (new Uri(GetType().Assembly.CodeBase).AbsolutePath);
      var sampleConfigLocation = Path.Combine (orignialAssemblyLocation, "Config\\SampleConfig.xml");
      var expectedConfigLocation = Path.Combine (Path.GetDirectoryName(configurationFileLocator.GetType().Assembly.Location), "config.injectioncop");

      try
      {
        File.Copy (sampleConfigLocation, expectedConfigLocation);

        var files = configurationFileLocator.GetFilesFromCurrentAssembly ();

        Assert.That (files, Contains.Item (Path.GetFullPath (expectedConfigLocation)));
      }
      finally
      {
        File.Delete (expectedConfigLocation);
      }
    }
    
    [Test]
    public void GetFilesFromCurrentAssembly_NoConfigFilesFound ()
    {
      var configurationFileLocator = new ConfigurationFileLocator();
      var files = configurationFileLocator.GetFilesFromCurrentAssembly ();

      Assert.That (files, Is.Empty);
    }
  }
}