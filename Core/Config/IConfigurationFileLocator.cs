using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Config
{
  public interface IConfigurationFileLocator
  {
    IEnumerable<string> GetFilesFromParsedType (TypeNode typeNode);
    IEnumerable<string> GetFilesFromCurrentAssembly ();
  }
}