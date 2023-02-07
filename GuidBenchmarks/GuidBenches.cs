using BenchmarkDotNet.Attributes;

namespace GuidBenchmarks;

public class GuidBenches
{
  private class SomeClass
  {
    public required string X { get; init; }
  }
  
  private Guid myGuid;
  private string myStringGuid;

  [IterationSetup]
  public void SetUp()
  {
    myGuid = Guid.NewGuid();
    myStringGuid = Guid.NewGuid().ToString();
  }


  [Benchmark]
  public void GuidToString()
  {
    var str = myGuid.ToString();
  }

  [Benchmark]
  public void ParseBenchmark()
  {
    var guid = Guid.Parse(myStringGuid);
  }
}