using BenchmarkDotNet.Attributes;

namespace LinkedList;

[MemoryDiagnoser]
public class SortingBenchmark
{
  public class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }

  [Params(10_000, 100_000, 1_000_000)] 
  public int ListCount;
  private LinkedList<Person> myList;

  
  [IterationSetup]
  public void SetUp()
  {
    myList = new LinkedList<Person>();
    for (var i = 0; i < ListCount; ++i)
    {
      myList.AddLast(new Person
      {
        Name = "Person",
        Age = Random.Shared.Next(1, 100)
      });
    }
  }

  [Benchmark]
  public LinkedList<Person> SortWithLinq()
  {
    return new LinkedList<Person>(myList.OrderByDescending(x => x.Age));
  }

  [Benchmark]
  public LinkedList<Person> SortWithArrayAllocation()
  {
    var array = myList.ToArray();
    Array.Sort(array, static (first, second) => second.Age - first.Age);
    return new LinkedList<Person>(array);
  }
}