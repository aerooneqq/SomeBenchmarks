using BenchmarkDotNet.Attributes;

namespace StructAllocations;

[MemoryDiagnoser]
public class StructAllocationsBenchmarks
{
  private readonly struct MyStruct
  {
    public int X { get; init; }
    public string Y { get; init; }
  }
  
  private class MyClass
  {
    public int X { get; init; }
    public string Y { get; init; }
  }

  private const int IterationCount = 1_000_000;

  [Benchmark]
  public void ListStructBenchmarks()
  {
    var list = new List<MyStruct>();
    for (int i = 0; i < IterationCount; i++)
    {
      list.Add(new MyStruct());
    }
  }
  
  [Benchmark]
  public void ObjectListStructBenchmarks()
  {
    var list = new List<object>();
    for (int i = 0; i < IterationCount; i++)
    {
      list.Add(new MyStruct());
    }
  }

  [Benchmark]
  public void ListClassBenchmark()
  {
    var list = new List<MyClass>();
    for (int i = 0; i < IterationCount; i++)
    {
      list.Add(new MyClass());
    }
  }
}