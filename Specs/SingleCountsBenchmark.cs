using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmarks.Specs;

public class SingleCountsEntity
{
    public int Property { get; set; }
}


public class SingleCountsBenchmark
{
    private IEnumerable<SingleCountsEntity> _entities = new List<SingleCountsEntity>();

    [GlobalSetup]
    public void Setup()
    {
        var faker = new Faker<SingleCountsEntity>().RuleForType(typeof(int), f => f.Random.Number(1, 10));
        _entities = faker.Generate(100);
    }

    [Benchmark]
    public void GetCountsLinq()
    {
        var result = _entities.Count(e => e.Property > 5);
    }

    [Benchmark]
    public void GetCountsForeach()
    {
        var result = 0;

        foreach (var entity in _entities)
        {
            if (entity.Property > 5) result++;
        }
    }

    [Benchmark]
    public void GetCountsAggregate()
    {
        var result = _entities.Aggregate(0, (count, entity) =>
        {
            if (entity.Property > 5) count++;
            return count;
        });
    }
}