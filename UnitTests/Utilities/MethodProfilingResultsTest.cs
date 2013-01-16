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
using InjectionCop.Utilities;
using NUnit.Framework;

namespace InjectionCop.UnitTests.Utilities
{
  [TestFixture]
  public class MethodProfilingResultsTest
  {
    private MethodProfilingResults _methodProfilingResults;

    [SetUp]
    public void SetUp ()
    {
      _methodProfilingResults = new MethodProfilingResults();
    }

    [Test]
    public void Add_EntryWithTimespanGreaterThanEpsilon_AddsEntry ()
    {
      _methodProfilingResults.Add ("A", new TimeSpan(0,0,0,0,10) );
      bool entryAdded = _methodProfilingResults.ToString().Contains ("0,01 s: A");
      Assert.That (entryAdded, Is.True);
    }

    [Test]
    public void Add_EntryWithTimespanSmallerThanEpsilon_DoesNotAddEntry ()
    {
      _methodProfilingResults.Add ("A", new TimeSpan(0,0,0,0,1) );
      bool entryAdded = _methodProfilingResults.ToString().Contains ("0,001 s: A");
      Assert.That (entryAdded, Is.False);
    }

    [Test]
    public void Add_C_EntriesOrderedDescending ()
    {
      _methodProfilingResults.Add ("A", new TimeSpan (0, 0, 0, 0, 9));
      _methodProfilingResults.Add ("B", new TimeSpan (0, 0, 0, 0, 10));
      int indexOfA = _methodProfilingResults.ToString().IndexOf("s: A", System.StringComparison.Ordinal);
      int indexOfB = _methodProfilingResults.ToString().IndexOf("s: B", System.StringComparison.Ordinal);
      
      bool correctOrder = indexOfA > indexOfB;
      Assert.That (correctOrder, Is.True);
    }

    [Test]
    public void Add_EntryWithTimespanSmallerThanEpsilon_Max10EntriesAreBuffered ()
    {
      for (int i = 0; i < 10; i++)
      {
        _methodProfilingResults.Add ("B", new TimeSpan (0, 0, 0, 0, 10));
      }
      _methodProfilingResults.Add ("A", new TimeSpan (0, 0, 0, 0, 9));
      
      bool containsA = _methodProfilingResults.ToString().Contains("s: A");
      Assert.That (containsA, Is.False);
    }
  }
}
