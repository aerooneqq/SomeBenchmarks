using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using BenchmarkDotNet.Attributes;

namespace Linq;

[MemoryDiagnoser(displayGenColumns: true)]
public class LinqBenchmarks
{
  private List<int> myList;
  private int[] myArray;

  [Params((int)1e2, (int)1e3, (int)1e4, (int)1e5, (int)1e6)]
  public int Length;


  [IterationSetup]
  public void Setup()
  {
    myList = Enumerable.Range(0, Length).Select(_ => Random.Shared.Next(0, 1_000)).ToList();
    myArray = myList.ToArray();
  }


  [Benchmark]
  public int SumListFor()
  {
    var sum = 0;
    
    // ReSharper disable once ForCanBeConvertedToForeach
    // ReSharper disable once LoopCanBeConvertedToQuery
    for (var i = 0; i < myList.Count; i++)
    {
      sum += myList[i];
    }

    return sum;
  }

  [Benchmark]
  public int SumListForeach()
  {
    var sum = 0;
    
    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var item in myList)
    {
      sum += item;
    }

    return sum;
  }

  [Benchmark]
  public int SumListLinq()
  {
    return myList.Sum();
  }

  [Benchmark]
  public int SumArrayFor()
  {
    var sum = 0;
    
    // ReSharper disable once ForCanBeConvertedToForeach
    // ReSharper disable once LoopCanBeConvertedToQuery
    for (var i = 0; i < myArray.Length; i++)
    {
      sum += myArray[i];
    }

    return sum;
  }
  
  [Benchmark]
  public int SumArrayForeach()
  {
    var sum = 0;
    
    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var item in myArray)
    {
      sum += item;
    }

    return sum;
  }
  
  [Benchmark]
  public unsafe int SumArrayVector()
  {
    var vector = Vector128<int>.Zero;
    int index;

    fixed (int* ptr = myArray)
    {
      for (index = 0; index + 4 < myArray.Length; index += 4)
      {
        var currentVector = AdvSimd.LoadVector128(ptr + index);
        vector = AdvSimd.Add(vector, currentVector);
      }
    }
    
    var sum = Vector128.Sum(vector);
    for (var i = index; i < myList.Count; ++i)
    {
      sum += myList[i];
    }
    
    return sum;
  }
}