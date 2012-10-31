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
using Microsoft.FxCop.Sdk;

namespace InjectionCop
{
    class PrettyPrint
    {
        public static string Print(Problem problem)
        {
            return "Found problem! " +
                   "\n Id: " + problem.Id +
                   "\n Certainty: " + problem.Certainty +
                   "\n FixCategory: " + problem.FixCategory.ToString() +
                   "\n MessageLevel: " + problem.MessageLevel.ToString() +
                   "\n Resolution: " + problem.Resolution +
                   "\n SourceFile: " + problem.SourceFile +
                   "\n SourceLine: " + problem.SourceLine;
        }

        public static string Print(AssignmentStatement asgn)
        {
            return  "sourceexpression type: " + asgn.Source.Type.Name + "\n" +
                    "operator: " + asgn.Operator.ToString() + "\n" +
                    "targetexpression type: " + asgn.Target.NodeType.ToString();
        }

        public static string Print(MethodCall mtc)
        {
            if (mtc.Callee is MemberBinding)
            {
                MemberBinding mb = (MemberBinding)mtc.Callee;
                if (mb.BoundMember is Method)
                {
                    Method calleeMethod = (Method) mb.BoundMember;
                    string output = "Calling: " + calleeMethod.FullName + "\nOperands: ";
                    foreach (Expression operand in mtc.Operands)
                    {
                        output += operand.Type.Name + "\n";
                    }
                    return output;
                }
            }
            return "cannot identify callee method";
        }

        public static string Print(Block block)
        {
            return Print("", block);
        }

        public static string Print(string indendation, Block block)
        {
            string output = "";
            foreach (Statement stmt in block.Statements)
            {
                if (stmt is Block)
                {
                    output += "Block{\n" +
                              Print(indendation + "  ", (Block)stmt) +
                              "}\n";
                }
                else
                {
                    output += indendation + stmt.NodeType.ToString() + "\n";
                }
            }
            return output;
        }

        public static string Print (string indendation, Method method)
        {
          string output = "Method: " + method.Name + "\n";
          foreach (Statement topLevelStatement in method.Body.Statements)
          {
            Block methodBodyBlock = topLevelStatement as Block;
            if (methodBodyBlock == null)
              continue;

            output += Print (indendation, methodBodyBlock);
          }
          return output;
        }
    }
}
