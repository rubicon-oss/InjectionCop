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

namespace InjectionCop.IntegrationTests.Parser.BlockParserTests.AssignmentPropagation
{
  class AssignmentPropagationSample: ParserSampleBase
  {
    public void ValidSafenessPropagation()
    {
      string temp = "select * from users";
      IDbCommand command = new SqlCommand();
      command.CommandText = temp;
    }

    public void InvalidSafenessPropagationParameter([Fragment("SqlFragment")] string temp)
    {
      temp = UnsafeSource(temp);
      IDbCommand command = new SqlCommand();
      command.CommandText = temp;
    }

    public void ValidSafenessPropagationParameter(string temp)
    {
      DummyMethod (temp);
      temp = SafeSource();
      IDbCommand command = new SqlCommand();
      command.CommandText = temp;
    }

    public void InvalidSafenessPropagationVariable()
    {
      string temp = SafeSource();
      temp = UnsafeSource(temp);
      IDbCommand command = new SqlCommand();
      command.CommandText = temp;
    }

    public void ValidSafenessPropagationVariable()
    {
      // ReSharper disable RedundantAssignment
      string temp = UnsafeSource();
      // ReSharper restore RedundantAssignment
      temp = SafeSource();
      IDbCommand command = new SqlCommand();
      command.CommandText = temp;
    }
  }
}
