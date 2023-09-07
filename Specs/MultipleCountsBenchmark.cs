using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmarks.Specs;

public class MultipleCountsEntity
{
    public int PropertyA { get; set; }
    public int PropertyB { get; set; }
    public int PropertyC { get; set; }
    public int PropertyD { get; set; }
}

public record MultipleCountsResult
{
    public int PropertyA { get; set; }
    public int PropertyB { get; set; }
    public int PropertyC { get; set; }
    public int PropertyD { get; set; }
}

public class MultipleCountsBenchmark
{
    private IEnumerable<MultipleCountsEntity> _entities = new List<MultipleCountsEntity>();

    [GlobalSetup]
    public void Setup()
    {
        var faker = new Faker<MultipleCountsEntity>().RuleForType(typeof(int), f => f.Random.Number(1, 10));
        _entities = faker.Generate(100);
    }

    [Benchmark]
    public void GetCountsLinq()
    {
        var result = new MultipleCountsResult
        {
            PropertyA = _entities.Count(e => e.PropertyA > 5),
            PropertyB = _entities.Count(e => e.PropertyB > 5),
            PropertyC = _entities.Count(e => e.PropertyC > 5),
            PropertyD = _entities.Count(e => e.PropertyD > 5)
        };
    }

    [Benchmark]
    public void GetCountsForeach()
    {
        var result = new MultipleCountsResult();

        foreach (var entity in _entities)
        {
            if (entity.PropertyA > 5) result.PropertyA++;
            if (entity.PropertyB > 5) result.PropertyB++;
            if (entity.PropertyC > 5) result.PropertyC++;
            if (entity.PropertyD > 5) result.PropertyD++;
        }
    }

    [Benchmark]
    public void GetCountsAggregate()
    {
        var result = _entities.Aggregate(new MultipleCountsResult(), (e, entity) =>
        {
            if (entity.PropertyA > 5) e.PropertyA++;
            if (entity.PropertyB > 5) e.PropertyB++;
            if (entity.PropertyC > 5) e.PropertyC++;
            if (entity.PropertyD > 5) e.PropertyD++;
            return e;
        });
    }
}