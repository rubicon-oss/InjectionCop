using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Config
{
  public class ConfigurationFactory
  {
    public static IBlacklistManager CreateFrom (TypeNode typeNode, IConfigurationFileLocator configurationFileLocator)
    {
      var configurationLayers = new Stack<IBlacklistManager>();

      LoadConfigurationLayer(configurationLayers, configurationFileLocator.GetFilesFromCurrentAssembly ());
      LoadConfigurationLayer(configurationLayers, configurationFileLocator.GetFilesFromParsedType (typeNode));

      return new LayeredConfigurationAdapter (configurationLayers);
    }

    private static void LoadConfigurationLayer (Stack<IBlacklistManager> configurationLayers, IEnumerable<string> files)
    {
      var configuration = new Configuration();

      bool hasConfigurationFiles = false;
      foreach (var file in files)
      {
        hasConfigurationFiles = true;
        configuration.LoadXml (File.ReadAllText (file));
      }

      if (hasConfigurationFiles)
        configurationLayers.Push (configuration);
    }
  }
}