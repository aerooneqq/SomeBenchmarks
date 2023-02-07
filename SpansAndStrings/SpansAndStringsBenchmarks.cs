using BenchmarkDotNet.Attributes;

namespace SpansAndStrings;

[MemoryDiagnoser]
public class SpansAndStringsBenchmarks
{
  private string myBenchmarkString;
  private string myStartsWithString;
  private string myStringToFind;

  [Params(100, 1_000, 10_000, 100_000)]
  public int Length;


  [IterationSetup]
  public void SetUp()
  {
    char[] GenerateCharArray(int length) =>
      Enumerable.Range(0, length).Select(_ => (char) Random.Shared.Next('a', 'z')).ToArray();

    myBenchmarkString = new string(GenerateCharArray(Length));

    var startsWithArray = myBenchmarkString.ToArray();
    startsWithArray[^1] = 'X';
    myStartsWithString = new string(startsWithArray);

    var stringToFindArray = GenerateCharArray(5);
    stringToFindArray[^1] = 'X';
    myStringToFind = new string(stringToFindArray);
  }

  [Benchmark]
  public int ForeachString()
  {
    var sum = 0;
    foreach (var c in myBenchmarkString)
    {
      sum += c;
    }

    return sum;
  }

  [Benchmark]
  public int ForeachSpan()
  {
    var sum = 0;
    foreach (var c in myBenchmarkString.AsSpan())
    {
      sum += c;
    }

    return sum;
  }

  [Benchmark]
  public bool StringContains()
  {
    return myBenchmarkString.Contains('F');
  }

  [Benchmark]
  public bool SpanContains()
  {
    return myBenchmarkString.AsSpan().Contains('F');
  }

  [Benchmark]
  public bool StringStartsWithCurrentCulture()
  {
    return myBenchmarkString.StartsWith(myStartsWithString, StringComparison.CurrentCulture);
  }
  
  [Benchmark]
  public bool StringStartsWithCurrentCultureIgnoreCase()
  {
    return myBenchmarkString.StartsWith(myStartsWithString, StringComparison.CurrentCultureIgnoreCase);
  }

  [Benchmark]
  public bool StringStartsWithOrdinal()
  {
    return myBenchmarkString.StartsWith(myStartsWithString, StringComparison.Ordinal);
  }

  [Benchmark]
  public bool SpanStartsWithOrdinal()
  {
    return myBenchmarkString.AsSpan().StartsWith(myStartsWithString.AsSpan(), StringComparison.Ordinal);
  }
  
  [Benchmark]
  public bool SpanStartsWithCurrentCulture()
  {
    return myBenchmarkString.AsSpan().StartsWith(myStartsWithString.AsSpan(), StringComparison.CurrentCulture);
  }

  [Benchmark]
  public int StringIndexOf()
  {
    return myBenchmarkString.IndexOf('X');
  }

  [Benchmark]
  public int SpanBenchmark()
  {
    return myBenchmarkString.AsSpan().IndexOf('X');
  }

  [Benchmark]
  public int StringIndexOfStringCurrentCulture()
  {
    return myBenchmarkString.IndexOf(myStringToFind, StringComparison.CurrentCulture);
  }

  [Benchmark]
  public int StringIndexOfStringOrdinal()
  {
    return myBenchmarkString.IndexOf(myStringToFind, StringComparison.Ordinal);
  }

  [Benchmark]
  public int SpanIndexOfSpanCurrentCulture()
  {
    return myBenchmarkString.AsSpan().IndexOf(myStringToFind.AsSpan(), StringComparison.CurrentCulture);
  }

  [Benchmark]
  public int SpanIndexOfSpanOrdinal()
  {
    return myBenchmarkString.AsSpan().IndexOf(myStringToFind.AsSpan(), StringComparison.Ordinal);
  }
}