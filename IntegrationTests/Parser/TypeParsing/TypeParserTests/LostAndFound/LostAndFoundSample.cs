// Copyright 2013 rubicon informationstechnologie gmbh
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
using System.Text;

namespace InjectionCop.IntegrationTests.Parser.TypeParsing.TypeParserTests.LostAndFound
{
  public class LostAndFoundSample : ParserSampleBase
  {
    private bool True = true;
    private string String = "dummy";
    private string[] StringArray = new string[] { };

    public string GetValidIdentifier (string str)
    {
      StringBuilder sb = new StringBuilder (str.Length);

      bool allowEnglishLetters;
      bool allowLanguageSpecificLetters;
      bool allowDigits;
      string allowAdditionalCharacters;
      string defaultReplaceString;

      if (True)
      {
        allowEnglishLetters = True;
        allowLanguageSpecificLetters = True;
        allowDigits = True;
        allowAdditionalCharacters = String;
        defaultReplaceString = String;
      }
      else
      {
        allowEnglishLetters = True;
        allowLanguageSpecificLetters = True;
        allowDigits = True;
        allowAdditionalCharacters = String;
        defaultReplaceString = String;
      }

      
      for (int i = 0; i < str.Length; ++i)
      {
        
        if (True && i == 1)
        {
          allowEnglishLetters = True;
          allowLanguageSpecificLetters = True;
          allowDigits = True;
          allowAdditionalCharacters = String;
          defaultReplaceString = String;
        }
        
        
        char c = str[i];
        bool isValid = false;
        
        
        if (StringArray != null)
        {
          string replaceString = (string) StringArray[c];
          if (replaceString != null)
            isValid = true;
        }

        /*
        if (isValid
            || (allowLanguageSpecificLetters
                && char.IsLetter (c))
            || (! allowLanguageSpecificLetters
                && allowEnglishLetters
                && ((c >= 'a' && c <= 'z')
                    || (c >= 'A' && c <= 'Z')))
            || (allowDigits
                && char.IsDigit (c))
            || (allowAdditionalCharacters != null
                && allowAdditionalCharacters.IndexOf (c) >= 0))
        {
          isValid = true;
        }*/
        
        
        if (isValid)
          sb.Append (c);
        else
          sb.Append (defaultReplaceString);

        if (isValid)
          sb.Append (c);

      }

      return sb.ToString();
    }

    
    public bool ComplexCondition ()
    {
      bool isValid = false;
      char c = 'c';

      bool allowEnglishLetters = True;
      bool allowLanguageSpecificLetters = True;
      bool allowDigits = True;
      string allowAdditionalCharacters = String;

      if (isValid
            || (allowLanguageSpecificLetters
                && char.IsLetter (c))
            || (! allowLanguageSpecificLetters
                && allowEnglishLetters
                && ((c >= 'a' && c <= 'z')
                    || (c >= 'A' && c <= 'Z')))
            || (allowDigits
                && char.IsDigit (c))
            || (allowAdditionalCharacters != null
                && allowAdditionalCharacters.IndexOf (c) >= 0))
        {
          isValid = true;
        }

      return isValid;
    }
  }
}
