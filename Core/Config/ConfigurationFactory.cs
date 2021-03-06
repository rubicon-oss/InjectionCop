﻿// Copyright 2013 rubicon informationstechnologie gmbh
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

using System.Collections.Generic;
using System.IO;
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