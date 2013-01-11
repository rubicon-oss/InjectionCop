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