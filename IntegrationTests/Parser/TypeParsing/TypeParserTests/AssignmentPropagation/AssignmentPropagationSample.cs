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

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.AssignmentPropagation
{
  class AssignmentPropagationSample: ParserSampleBase
  {
    public void ValidSafenessPropagation()
    {
      string temp = "select * from users";
      RequiresSqlFragment (temp);
    }

    public void InvalidSafenessPropagationParameter([Fragment("SqlFragment")] string temp)
    {
      temp = UnsafeSource(temp);
      RequiresSqlFragment (temp);
    }

    public void ValidSafenessPropagationParameter(string temp)
    {
      DummyMethod (temp);
      temp = SafeSource();
      RequiresSqlFragment (temp);
    }

    public void InvalidSafenessPropagationVariable()
    {
      string temp = SafeSource();
      temp = UnsafeSource(temp);
      RequiresSqlFragment (temp);
    }

    public void ValidSafenessPropagationVariable()
    {
      // ReSharper disable RedundantAssignment
      string temp = UnsafeSource();
      // ReSharper restore RedundantAssignment
      temp = SafeSource();
      RequiresSqlFragment (temp);
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithIf ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue;
      if (SafeSource() == "Dummy")
      {
        returnValue = "safe";
      }
      else
      {
        returnValue = parameter;
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithIf ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue;
      if (SafeSource() == "Dummy")
      {
        returnValue = "safe";
      }
      else
      {
        returnValue = UnsafeSource();
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithIfFragmentTypeConsidered ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod("dummy");
      }
      else
      {
        returnValue = UnsafeSource();
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithTempVariable ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod("dummy");
      }
      else
      {
        string temp = UnsafeSource();
        DummyMethod (temp);
        returnValue = temp;
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithParameterReset ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod ("dummy");
      }
      else
      {
        parameter = UnsafeSource();
        returnValue = parameter;
      }
      return returnValue;
    }

    [return: Fragment("DummyFragment")]
    private string SafeDummyFragmentSource ()
    {
      return "safe";
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithParameterReset ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod ("dummy");
      }
      else
      {
        parameter = SafeDummyFragmentSource();
        returnValue = parameter;
      }
      return returnValue;
    }

    [Fragment("DummyFragment")]
    private string _dummyFragmentField = "";
    
    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithFieldReset ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod ("dummy");
      }
      else
      {
        _dummyFragmentField = UnsafeSource();
        returnValue = _dummyFragmentField;
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithFieldReset ([Fragment ("DummyFragment")] string parameter)
    {
      string returnValue = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod ("dummy");
      }
      else
      {
        _dummyFragmentField = SafeDummyFragmentSource();
        returnValue = _dummyFragmentField;
      }
      return returnValue;
    }

    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithField ([Fragment ("DummyFragment")] string parameter)
    {
      _dummyFragmentField = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod("dummy");
      }
      else
      {
        string temp = UnsafeSource();
        DummyMethod (temp);
        _dummyFragmentField = temp;
      }
      return _dummyFragmentField;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithField ([Fragment ("DummyFragment")] string parameter)
    {
      _dummyFragmentField = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod("dummy");
      }
      else
      {
        string temp = SafeDummyFragmentSource();
        DummyMethod (temp);
        _dummyFragmentField = temp;
      }
      return _dummyFragmentField;
    }

    /// <summary>
    /// because InjectionCop does not support analyzing while conditions 
    /// this sample is declared invalid although an unsafe value never gets
    /// assigned to fragmentField
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    [return: Fragment ("DummyFragment")]
    public string InvalidReturnWithFieldAndLoops ([Fragment ("DummyFragment")] string parameter)
    {
      _dummyFragmentField = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod("dummy");
      }
      else
      {
        string temp = SafeDummyFragmentSource();
        DummyMethod (temp);
        int i = 0;
        while (i < 10)
        {
          while (i < 5)
          {
            DummyMethod (temp);
            _dummyFragmentField = temp;
            temp = SafeDummyFragmentSource();
            i++;
          }
          temp = UnsafeSource();
          i++;
        }
      }
      return _dummyFragmentField;
    }

    [return: Fragment ("DummyFragment")]
    public string ValidReturnWithFieldAndLoops ([Fragment ("DummyFragment")] string parameter)
    {
      _dummyFragmentField = parameter;
      if (SafeSource() == "Dummy")
      {
        DummyMethod("dummy");
      }
      else
      {
        string temp = SafeDummyFragmentSource();
        DummyMethod (temp);
        int i = 0;
        while (i < 10)
        {
          while (i < 5)
          {
            DummyMethod (temp);
            _dummyFragmentField = temp;
            temp = SafeDummyFragmentSource();
            i++;
          }
          temp = "safe";
          i++;
        }
      }
      return _dummyFragmentField;
    }
  }
}
