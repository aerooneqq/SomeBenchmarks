using BenchmarkDotNet.Attributes;

namespace Dictionary;


public record SomeEntity(int Age, string Name)
{
  public static SomeEntity CreateRandom() => new(Random.Shared.Next(1, 100), Guid.NewGuid().ToString());
}

public class DictionaryVsArrayAccessSpeed
{
  [Params(5, 10, 20, 30, 40, 50, 100, 500, 1000, 10_000)]
  public int UniqueElementsCount;

  private List<string> myNames;
  private KeyValuePair<string, SomeEntity>[] myArray;
  private Dictionary<string, SomeEntity> myDictionary;
  private List<SomeEntity> myResults;


  [IterationSetup]
  public void Setup()
  {
    myNames = new List<string>();
    myArray = new KeyValuePair<string, SomeEntity>[UniqueElementsCount];
    myDictionary = new Dictionary<string, SomeEntity>();
    myResults = new List<SomeEntity>();
    
    for (var i = 0; i < UniqueElementsCount; i++)
    {
      var entity = SomeEntity.CreateRandom();
      myArray[i] = new KeyValuePair<string, SomeEntity>(entity.Name, entity);
      myDictionary[entity.Name] = entity;
      myNames.Add(entity.Name);
    }

    for (var i = 0; i < 1000; i++)
    {
      var firstIndex = Random.Shared.Next(0, myNames.Count);
      var secondIndex = Random.Shared.Next(0, myNames.Count);

      (myNames[firstIndex], myNames[secondIndex]) = (myNames[secondIndex], myNames[firstIndex]);
    }
  }

  [Benchmark]
  public void TestAccessToDictionaryIndexer()
  {
    foreach (var name in myNames)
    {
      myResults.Add(myDictionary[name]);
    }
  }

  [Benchmark]
  public void TestAccessToDictionaryTryGetValue()
  {
    foreach (var name in myNames)
    {
      if (myDictionary.TryGetValue(name, out var entity))
      {
        myResults.Add(entity);
      }
    }
  }

  [Benchmark]
  public void TestAccessToArray()
  {
    foreach (var name in myNames)
    {
      for (var i = 0; i < myArray.Length; i++)
      {
        var pair = myArray[i];
        if (pair.Value.Name == name)
        {
          myResults.Add(pair.Value);
          break;
        }
      }
    }
  }
}

[MemoryDiagnoser]
public class DictionaryVsArrayAllocation
{
  private List<SomeEntity> myEntities;

  [Params(5, 10, 20, 30, 40, 50, 100, 500, 1000, 10_000)]
  public int UniqueElementsCount;
  
  [IterationSetup]
  public void SetUp()
  {
    myEntities = new List<SomeEntity>();
    for (int i = 0; i < UniqueElementsCount; i++)
    {
      myEntities.Add(SomeEntity.CreateRandom());
    }
  }
  
  [Benchmark]
  public void DictionaryAllocation()
  {
    Dictionary<string, SomeEntity> dictionary = new(UniqueElementsCount);
    for (var i = 0; i < myEntities.Count; i++)
    {
      var entity = myEntities[i];
      dictionary[entity.Name] = entity;
    }
  }

  [Benchmark]
  public void ArrayAllocation()
  {
    var array = new KeyValuePair<string, SomeEntity>[UniqueElementsCount];
    for (var i = 0; i < myEntities.Count; i++)
    {
      var value = myEntities[i];
      array[i] = new KeyValuePair<string, SomeEntity>(value.Name, value);
    }
  }
}