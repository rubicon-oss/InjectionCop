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

namespace InjectionCop.Config
{
  public class IDbCommandBlacklistManagerStub : IBlacklistManager
  {
    public bool IsListed (string qualifiedTypeName, string methodName, List<string> qualifiedParameterTypes)
    {
      return qualifiedTypeName == "System.Data.IDbCommand"
             && methodName == "set_CommandText"
             && qualifiedParameterTypes.Count == 1
             && qualifiedParameterTypes[0] == "System.String";
    }

    public List<string> GetFragmentTypes (string qualifiedTypeName, string methodName, List<string> qualifiedParameterTypes)
    {
      return new List<string> { "SqlFragment" };
    }
  }
}
