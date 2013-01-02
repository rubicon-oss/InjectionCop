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
using InjectionCop.Fragment;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Parameter.CallByReference
{
  class CallByReferenceSample: ParserSampleBase
  {
    public void FragmentRefParameterSafeCall ()
    {
      string safe = "safe";
      FragmentRefParameter (ref safe, 0);
    }

    public void FragmentRefParameterUnsafeCall ()
    {
      string unSafe = UnsafeSource();
      FragmentRefParameter (ref unSafe, 0);
    }
    
    public void RefParameterSafeOperand()
    {
      string safe = SafeSource();
      FragmentRefParameter (ref safe, 0);
      RequiresSqlFragment (safe);
    }

    public void RefParameterUnsafeOperand()
    {
      string unSafe = UnsafeSource();
      NonFragmentRefParameter (ref unSafe, 0);
      RequiresSqlFragment (unSafe);
    }

    public void RefParameterSafeVariableTurningUnsafe()
    {
      string turnUnsafe = SafeSource();
      NonFragmentRefParameter (ref turnUnsafe, 0);
      RequiresSqlFragment (turnUnsafe);
    }

    private void FragmentRefParameter([Fragment("SqlFragment")] ref string safe, int dummy)
    {
      safe = "safe" + safe + dummy;
    }

    private void NonFragmentRefParameter(ref string unSafe, int dummy)
    {
      unSafe = unSafe + dummy;
    }
  }
}
