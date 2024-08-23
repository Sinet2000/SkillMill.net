using BenchmarkDotNet.Attributes;

namespace SkillMill.Application.Problems;

public class TrackingProblem
{
    private AppDbContext dbContext;

    [GlobalSetup]
    public void Init()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(DataConst.CoreDbConnectionString);
        dbContext = new AppDbContext(optionsBuilder.Options);
    }

    [Benchmark]
    // This query tracks the entities and changes to them. Useful when you need to update entities.
    public async Task AsTracking()
    {
        var customers = await dbContext.Customers.ToListAsync().ConfigureAwait(false);
    }

    [Benchmark]
    // This query does not track the entities, improving performance for read-only scenarios.
    public async Task AsNoTracking()
    {
        var customers = await dbContext.Customers.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    [Benchmark]
    // This query does not track entities but ensures that entities with the same key are represented as a single instance.
    public async Task AsNoTrackingWithIdentityResolution()
    {
        var customers = await dbContext.Customers.AsNoTrackingWithIdentityResolution().ToListAsync().ConfigureAwait(false);
    }

    [Benchmark]
    // This query tracks entities and ensures that entities with the same key are represented as a single instance in memory.
    public async Task AsTrackingWithIdentityResolution()
    {
        var customers = await dbContext.Customers.AsTracking().AsNoTrackingWithIdentityResolution().ToListAsync().ConfigureAwait(false);
    }
}