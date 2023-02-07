using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Iterations;

public class IterationsBenchmark
{
  private class SomeEntity
  {
    public static SomeEntity CreateRandomEntity() => new()
    {
      X = Random.Shared.Next(0, 100),
      Name = new string('a', Random.Shared.Next(20))
    };
    

    public required int X { get; init; }
    public required string Name { get; init; }
  }
  
  private List<SomeEntity> myEntities;

  [Params(100, 1_000, 10_000, 100_000, 1_000_000)]
  public int ListSize;

  [IterationSetup]
  public void SetUp()
  {
    myEntities = new List<SomeEntity>();
    for (var i = 0; i < ListSize; i++)
    {
      myEntities.Add(SomeEntity.CreateRandomEntity());
    }
  }

  [Benchmark]
  public void IterateListFor()
  {
    var sum = 0;
    
    // ReSharper disable once ForCanBeConvertedToForeach
    for (var i = 0; i < myEntities.Count; i++)
    {
      sum += myEntities[i].X;
    }
  }

  [Benchmark]
  public void IterateListForeach()
  {
    var sum = 0;
    foreach (var entity in myEntities)
    {
      sum += entity.X;
    }
  }

  [Benchmark]
  public void IterateSpanFor()
  {
    var span = CollectionsMarshal.AsSpan(myEntities);
    var sum = 0;
    
    for (int i = 0; i < span.Length; i++)
    {
      sum += span[i].X;
    }
  }

  [Benchmark]
  public void IterateSpanForeach()
  {
    var span = CollectionsMarshal.AsSpan(myEntities);
    var sum = 0;
    
    foreach (var entity in span)
    {
      sum += entity.X;
    }
  }
}