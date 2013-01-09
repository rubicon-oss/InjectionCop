using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Config
{
  public class ConfigurationFileLocator : IConfigurationFileLocator
  {
    private const string c_searchPattern = "*.injectioncop";

    public IEnumerable<string> GetFilesFromParsedType (TypeNode typeNode)
    {
      var assemblyDirectory = Path.GetDirectoryName (typeNode.DeclaringModule.ContainingAssembly.Location);

      return Directory.GetFiles (assemblyDirectory, c_searchPattern);
    }

    public IEnumerable<string> GetFilesFromCurrentAssembly ()
    {
      var assemblyDirectory = Path.GetDirectoryName (GetType().Assembly.Location);

      return Directory.GetFiles (assemblyDirectory, c_searchPattern);
    }
  }
}