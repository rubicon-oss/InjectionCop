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

namespace InjectionCop.IntegrationTests.Parser.Parameter
{
  class ParameterSample: TypeParserSample
  {
    public void FragmentOutParameterSafe()
    {
      string makeSafe;
      FragmentOutParameter (out makeSafe);
      RequiresSqlFragment(makeSafe);
    }

    public void OutParameterUnsafe()
    {
      string unSafe;
      OutParameter(out unSafe);
      RequiresSqlFragment(unSafe);
    }

    private void FragmentOutParameter([SqlFragment] out string safe)
    {
      safe = "safe";
    }

    private void OutParameter(out string unSafe)
    {
      unSafe = "unsafe";
    }
  }
}
