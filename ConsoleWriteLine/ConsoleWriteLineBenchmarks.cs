using BenchmarkDotNet.Attributes;

namespace ConsoleWriteLine;

[MemoryDiagnoser]
public class ConsoleWriteLineBenchmarks
{
  [Benchmark]
  public string DoFirstBenchmark()
  {
    var x = 5;
    object o = x;
    var y = 123;
    return y + "," + (int) o;
  }
  
  [Benchmark]
  public string DoSecondBenchmark()
  {
    var x = 5;
    object o = x;
    var y = 123;
    return y + "," + o;
  }
}