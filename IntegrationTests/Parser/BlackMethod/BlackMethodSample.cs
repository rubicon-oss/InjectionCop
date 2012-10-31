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

namespace InjectionCop.IntegrationTests.Parser.BlackMethod
{
  class BlackMethodSample: TypeParserSample
  {
    public void BlackMtcLiteral()
    {
      IDbCommand command = new SqlCommand();
      command.CommandText = "select * from users";
    }

    public void BlackMtcSafeSource()
    {
      IDbCommand command = new SqlCommand();
      command.CommandText = SafeSource();
    }

    public void BlackMtcUnsafeSourceNoParameter()
    {
      IDbCommand command = new SqlCommand();
      command.CommandText = UnsafeSource();
    }

    public void BlackMtcUnsafeSourceWithSafeParameter()
    {
      IDbCommand command = new SqlCommand();
      command.CommandText = UnsafeSource("");
    }

    public void WhiteMtc()
    {
      IDbCommand command = new SqlCommand();
      command.CommandTimeout = 0;
      command.Dispose();
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandTimeout = 0;
      sqlCommand.Dispose();
      string str;
      str = "a" + "b";
      if(str=="ab") { }
    }
  }
}
