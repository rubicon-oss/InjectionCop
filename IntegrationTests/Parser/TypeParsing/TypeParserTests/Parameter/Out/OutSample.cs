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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.Parameter.Out
{
  class OutSample: ParserSampleBase
  {
    public void FragmentOutParameterSafeCall()
    {
      // ReSharper disable RedundantAssignment
      string staySafe = SafeSource();
      // ReSharper restore RedundantAssignment
      FragmentOutParameterSafeReturn (out staySafe);
    }

    public void FragmentOutParameterUnsafeCall()
    {
      // ReSharper disable RedundantAssignment
      string unsafeVariable = UnsafeSource();
      // ReSharper restore RedundantAssignment
      FragmentOutParameterSafeReturn (out unsafeVariable);
    }

    public void OutParameterSafeOperand()
    {
      // ReSharper disable RedundantAssignment
      string operand = SafeSource();
      // ReSharper restore RedundantAssignment
      FragmentOutParameterSafeReturn(out operand);
      RequiresSqlFragment(operand);
    }
    
    public void OutParameterUnsafeOperand()
    {
      // ReSharper disable RedundantAssignment
      string unSafe = UnsafeSource();
      // ReSharper restore RedundantAssignment
      NonFragmentOutParameter(out unSafe);
      RequiresSqlFragment(unSafe);
    }

    public void OutParameterSafeVariableTurningUnsafe()
    {
      // ReSharper disable RedundantAssignment
      string turnsUnsafe = SafeSource();
      // ReSharper restore RedundantAssignment
      NonFragmentOutParameter(out turnsUnsafe);
      RequiresSqlFragment(turnsUnsafe);
    }

    private void FragmentOutParameterSafeReturn([Fragment("SqlFragment")] out string safe)
    {
      safe = "safe";
    }
    
    public void FragmentOutParameterUnsafeReturn([Fragment("SqlFragment")] out string unSafe)
    {
      unSafe = UnsafeSource();
    }

    [return: SqlFragment]
    public string FragmentOutParameterUnsafeReturnWithAssignment([Fragment("SqlFragment")] out string unSafe)
    {
      unSafe = UnsafeSource();
      string temp = unSafe;
      unSafe = SafeSource();
      return temp;
    }
   
    // sample mit return in der condition


    private void NonFragmentOutParameter(out string unSafe)
    {
      unSafe = "unsafe";
    }

  }
}
