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
using System.Data;
using System.Data.SqlClient;
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing
{
  public class BlacklistManagerIntegrationSample : ParserSampleBase
  {
    public void UnsafeBlacklistedCall ()
    {
      IDbCommand command = new SqlCommand();
      command.CommandText = UnsafeSource ("");
    }

    public void SafeBlacklistedCall ()
    {
      IDbCommand command = new SqlCommand();
      command.CommandText = SafeSource();
    }

    public void ListedAndUnlistedViolation ()
    {
      RequiresSqlFragment (UnsafeSource());
      IDbCommand command = new SqlCommand();
      command.CommandText = UnsafeSource ("");
    }

    public void FragmentDefinedInXmlSafeCall ()
    {
      FragmentDefinedInXml (ReturnsExternal());
    }

    public void FragmentDefinedInXmlUnsafeCall ()
    {
      FragmentDefinedInXml (UnsafeString());
    }

    public void MixedViolations (int x)
    {
      RequiresDummyFragment (3);
      RequiresDummyFragment (SafeDummySource());
      RequiresDummyFragment (UnsafeDummySource());
      RequiresDummyFragment(OtherUnsafeDummySource());
      IDbCommand command = new SqlCommand();
      command.CommandText = UnsafeString();

      FragmentDefinedInXml (UnsafeString());
      FragmentDefinedInXml (ReturnsExternal());

      RequiresSqlFragment ("safe");             // safe since literal

      if (1 == x)
      {
        if (2 == x)
        {
          //requiresFragment (OtherUnsafeSource());
        }
        else
        {
          //requiresFragment (OtherUnsafeSource());
        }
      }
    }

    public void RequiresDummyFragment ([Fragment ("dummy")] int i)
    {
    }
    
    public int UnsafeDummySource ()
    {
      return 3;
    }

    [return:Fragment("wrongtype")]
    public int OtherUnsafeDummySource ()
    {
      return 3;
    }

    [return:Fragment("dummy")]
    public int SafeDummySource ()
    {
      return 3;
    }

    public string UnsafeString ()
    {
      return "unsafe command";
    }

    [return: Fragment ("external")]
    public string ReturnsExternal ()
    {
      return "safe";
    }

    public void FragmentDefinedInXml (string needsExternalFragment)
    {
    }
  }
}
