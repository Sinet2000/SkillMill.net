using BenchmarkDotNet.Attributes;

namespace SkillMill.Application.Problems;

public class NPlusOneProblem
{
    private AppDbContext dbContext;

    [GlobalSetup]
    public void Init()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(DataConst.CoreDbConnectionString);
        dbContext = new AppDbContext(optionsBuilder.Options);
    }

    // Problem: Retrieving customers with orders using lazy loading causes the N+1 issue.
    // Each customer's orders are loaded individually, resulting in multiple queries and significant performance overhead.
    [Benchmark]
    public async Task Problem()
    {
        var customers = await dbContext.Customers.ToListAsync().ConfigureAwait(false);
        foreach (var customer in customers)
        {
            var orders = await dbContext.Orders.Where(d => d.CustomerId == customer.Id).ToListAsync().ConfigureAwait(false);
        }
    }

    // N+1 Problem Solution ✅: Use Include to load all related data in a single query
    [Benchmark]
    public async Task Solution()
    {
        var optimizedCustomers = await dbContext.Customers
            .Include(c => c.Orders)
            .ToListAsync().ConfigureAwait(false);
    }
}