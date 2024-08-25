using BenchmarkDotNet.Attributes;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Problems;

public class CartesianExplosionProblem
{
    private AppDbContext? dbContext;

    [GlobalSetup]
    public void Init()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(DataConst.CoreDbConnectionString);
        dbContext = new AppDbContext(optionsBuilder.Options);
    }

    // **Cartesian Explosion Problem**
    // Problem: Loading related data with multiple nested `Include` statements can cause a Cartesian Explosion.
    [Benchmark]
    public async Task CartesianExplosion()
    {
        var customers = await dbContext!.Customers
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);
    }

    // **Optimized Query to Avoid Cartesian Explosion**
    // ✅ Solution: Using Split Queries and Batched Queries to handle large datasets efficiently.

    [Benchmark]
    public async Task AsSplitAndNoTracking()
    {
        var customersSplitQueries = await dbContext!.Customers
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);
    }

    // ** Batched Queries Solution **
    //  ✅ Solution: This method processes customers in smaller batches to avoid loading a large dataset all at once.
    // By dividing the query into manageable chunks, it helps mitigate performance issues related to large result sets and prevents potential memory overload.
    [Benchmark]
    public async Task BatchedQueries()
    {
        var batchSize = 100;
        var customersFromBatches = new List<Customer>();
        int totalCustomers = await dbContext!.Customers.CountAsync();

        for (int i = 0; i < totalCustomers; i += batchSize)
        {
            var batch = await dbContext.Customers
                .AsNoTracking()
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Skip(i)
                .Take(batchSize)
                .ToListAsync().ConfigureAwait(false);

            customersFromBatches.AddRange(batch);
        }
    }

    // ** No Tracking Solution **
    // Using AsNoTracking to avoid overhead of change tracking.
    [Benchmark]
    public async Task NoTracking()
    {
        var customersNoTracking = await dbContext!.Customers
            .AsNoTracking()
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);
    }

    // ** Selective Loading Solution **
    // This method uses AsNoTracking and AsSplitQuery to optimize the performance by loading data in separate queries.
    // AsNoTracking avoids overhead by disabling change tracking, and AsSplitQuery splits the data retrieval into multiple queries 
    // to manage large datasets and avoid performance degradation caused by complex joins and Cartesian explosions.
    [Benchmark]
    public async Task SelectiveLoading()
    {
        var customers = await dbContext!.Customers
            .AsNoTrackingWithIdentityResolution()
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Email,
                Orders = c.Orders.Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    OrderItems = o.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        oi.Quantity,
                        Product = new
                        {
                            oi.Product.Id,
                            oi.Product.Name
                        }
                    })
                })
            }).ToListAsync().ConfigureAwait(false);
    }
}