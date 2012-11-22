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

namespace InjectionCop.IntegrationTests.Parser._Type.TypeParserTests.Parameter
{
  class ParameterSample: ParserSampleBase
  {
    public void UnsafeMethodParameter(string unsafeParam)
    {
      RequiresSqlFragment(unsafeParam);
    }

    public void SafeMethodParameter([Fragment("SqlFragment")]string safeParam)
    {
      RequiresSqlFragment(safeParam);
    }

    public void FragmentOutParameterSafe()
    {
      // ReSharper disable RedundantAssignment
      string staySafe = SafeSource();
      // ReSharper restore RedundantAssignment
      FragmentOutParameter (out staySafe);
      RequiresSqlFragment(staySafe);
    }

    public void FragmentOutParameterUnsafe()
    {
      // ReSharper disable RedundantAssignment
      string unSafe = UnsafeSource();
      // ReSharper restore RedundantAssignment
      FragmentOutParameter (out unSafe);
      RequiresSqlFragment(unSafe);
    }

    private void FragmentOutParameter([Fragment("SqlFragment")] out string safe)
    {
      safe = "safe";
    }

    public void OutParameterUnsafeOperand()
    {
      // ReSharper disable RedundantAssignment
      string unSafe = UnsafeSource();
      // ReSharper restore RedundantAssignment
      OutParameter(out unSafe);
      RequiresSqlFragment(unSafe);
    }

    public void OutParameterSafeOperand()
    {
      // ReSharper disable RedundantAssignment
      string turnUnsafe = SafeSource();
      // ReSharper restore RedundantAssignment
      OutParameter(out turnUnsafe);
      RequiresSqlFragment(turnUnsafe);
    }

    private void OutParameter(out string unSafe)
    {
      unSafe = "unsafe";
    }

    public void FragmentRefParameterSafe()
    {
      string staySafe = "";
      FragmentRefParameter (ref staySafe, 0);
      RequiresSqlFragment (staySafe);
    }

    public void FragmentRefParameterUnsafe()
    {
      string unSafe = UnsafeSource();
      FragmentRefParameter (ref unSafe, 0);
    }

    private void FragmentRefParameter([Fragment("SqlFragment")] ref string safe, int dummy)
    {
      safe = "safe" + safe + dummy;
    }

    public void RefParameterUnsafeOperand()
    {
      string unSafe = UnsafeSource();
      RefParameter (ref unSafe, 0);
      RequiresSqlFragment (unSafe);
    }

    public void RefParameterSafeOperand()
    {
      string turnUnsafe = SafeSource();
      RefParameter (ref turnUnsafe, 0);
      RequiresSqlFragment (turnUnsafe);
    }

    private void RefParameter(ref string unSafe, int dummy)
    {
      unSafe = unSafe + dummy;
    }

    public void ParameterOnly ([Fragment ("FragmentType")] int i)
    {
    }
  }

  public class ParameterSampleType
  {
    public void ParameterOnly (int i)
    {
    }
  }
}
