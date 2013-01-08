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
using System.Globalization;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.TryCatchFinally
{
  public class TryCatchFinallySample: ParserSampleBase
  {
    public void SafeCallInsideTry ()
    {
      try
      {
        RequiresSqlFragment("safe");
        ThrowsException(0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      DummyMethod ("dummy");
    }

    public void UnsafeCallInsideTry ()
    {
      try
      {
        RequiresSqlFragment(UnsafeSource());
        ThrowsException(0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      DummyMethod ("dummy");
    }

    public void SafeCallInsideCatch ()
    {
      try
      {
        ThrowsException (0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      catch (Exception ex)
      {
        DummyMethod (ex.Message);
        RequiresSqlFragment ("safe");
      }
      DummyMethod ("dummy");
    }

    public void UnsafeCallInsideCatch ()
    {
      try
      {
        ThrowsException (0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      catch (Exception ex)
      {
        RequiresSqlFragment (ex.Message);
      }
      DummyMethod ("dummy");
    }

    public void SafeCallInsideFinally ()
    {
      try
      {
        RequiresSqlFragment ("safe");
        ThrowsException (0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      finally
      {
        RequiresSqlFragment ("safe");
      }
      DummyMethod ("dummy");
    }

    public void UnsafeCallInsideFinally ()
    {
      try
      {
        RequiresSqlFragment ("safe");
        ThrowsException (0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      finally
      {
        RequiresSqlFragment (UnsafeSource());
      }
      DummyMethod ("dummy");
    }

    public void UnsafeCallNestedTry ()
    {
      try
      {
        ThrowsException(0);
        try
        {
          ThrowsException(0);
          RequiresSqlFragment(UnsafeSource());
        }
        catch (ArgumentNullException ex)
        {
          DummyMethod (ex.Message);
        }
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      DummyMethod ("dummy");
    }

    public void UnsafeCallNestedCatch ()
    {
      try
      {
        ThrowsException(0);
        }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
        try
        {
          ThrowsException(0);
          RequiresSqlFragment(UnsafeSource());
        }
        catch (ArgumentNullException e)
        {
          DummyMethod (e.Message);
        }
      }
      DummyMethod ("dummy");
    }

    public void UnsafeCallNestedFinally ()
    {
      try
      {
        ThrowsException (0);
      }
      catch (ArgumentNullException ex)
      {
        DummyMethod (ex.Message);
      }
      finally
      {
        try
        {
          ThrowsException(0);
          RequiresSqlFragment(UnsafeSource());
        }
        catch (ArgumentNullException e)
        {
          DummyMethod (e.Message);
        }
      }
      DummyMethod ("dummy");
    }

    private void ThrowsException (int parameter)
    {
      if (parameter == 1)
      {
        throw new ArgumentNullException();
      }
      DummyMethod (parameter.ToString(CultureInfo.InvariantCulture));
    }
  }
}
