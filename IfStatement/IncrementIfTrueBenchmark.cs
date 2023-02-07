using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace IfStatement;

public class IncrementIfTrueBenchmark
{
  private bool[] myArray;
  
  [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
  public int Length;

  [IterationSetup]
  public void SetUp()
  {
    myArray = new bool[Length];
    for (int i = 0; i < Length; i++)
    {
      myArray[i] = Random.Shared.Next(0, 2) >= 1;
    }
  }

  [Benchmark]
  public void IfStatement()
  {
    var counter = 0;
    for (var i = 0; i < Length; i++)
    {
      if (myArray[i])
      {
        ++counter;
      }
    }
  }

  [Benchmark]
  public unsafe void NotIfStatement()
  {
    var counter = 0;
    for (var i = 0; i < Length; i++)
    {
      counter += Unsafe.Read<byte>(Unsafe.AsPointer(ref myArray[i]));
    }
  }

  [Benchmark]
  public unsafe void NotIfStatementUnsafe()
  {
    var counter = 0;
    fixed (bool* first = &myArray[0])
    {
      var length = myArray.Length;
      var ptr = first;
      for (var i = 0; i < length; ++i)
      {
        counter += Unsafe.Read<byte>(ptr);
        ++ptr;
      } 
    }
  }
}