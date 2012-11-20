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
using InjectionCop.Attributes;

namespace InjectionCop.IntegrationTests.Parser
{
  class FragmentToolsSample
  {
    public void ContainsFragmentParameter([Fragment("FragmentType")] string parameter)
    {
    }

    public void NoFragmentParameter(string parameter)
    {
    }

    public void ContainsNonFragmentParameter([NonFragment("FragmentType")] string parameter)
    {
    }

    public void ContainsSqlFragmentParameter([Fragment("SqlFragment")] string parameter)
    {
    }

    public void ContainsStronglyTypedSqlFragmentParameter([SqlFragment] string parameter)
    {
    }
  }
}
