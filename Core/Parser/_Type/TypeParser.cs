﻿// Copyright 2012 rubicon informationstechnologie gmbh
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
using InjectionCop.Config;
using InjectionCop.Parser._Method;
using Microsoft.FxCop.Sdk;

namespace InjectionCop.Parser._Type
{
  /// <summary>
  /// Checks all members of a given type for safeness violations
  /// </summary>
  public class TypeParser : BaseFxCopRule
  {
    private IBlackTypes _blackTypes;

    public TypeParser ()
        : base ("TypeParser")
    {
      _blackTypes = new BlackTypesStub();
    }

    public override ProblemCollection Check (TypeNode type)
    {
      foreach (Member member in type.Members)
      {
        if(member is Method)
        {
          Method method = (Method) member;
          Parse(method);
        }
      }
      return Problems;
    }

    public void AddProblem ()
    {
      Resolution resolution = GetResolution();
      Problem problem = new Problem (resolution, CheckId);
      Problems.Add (problem);
    }

    public ProblemCollection Parse (Method method)
    {
      IMethodParser methodParser = new MethodParser (this);
      IMethodParserDirector director = new MethodParserDirector (method, _blackTypes, this);
      return methodParser.Parse (director);
    }
  }

  public class BlackTypesStub : IBlackTypes
  {
    public bool IsBlackType(string fullName)
    {
      return fullName == "System.Data.IDbCommand";
    }

    public bool IsBlackMethod (string typeFullName, string methodName)
    {
      return IsBlackType (typeFullName) && methodName == "set_CommandText";
    }
  }
}