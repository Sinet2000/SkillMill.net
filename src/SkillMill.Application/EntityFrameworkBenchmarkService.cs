using SkillMill.Domain.Entities;

namespace SkillMill.Application;

public class EntityFrameworkBenchmarkService
{
    private AppDbContext dbContext;


    public async Task CartesianExplosionAndSolution()
    {
        // **Cartesian Explosion Problem**
        var customers = await dbContext.Customers
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);

        // **Optimized Query to Avoid Cartesian Explosion**
        var splitQueries = await dbContext.Customers
            .AsSplitQuery()
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);

        // **Batched query*
        var batchSize = 100;
        var customersFromBatches = new List<Customer>();
        int totalCustomers = await dbContext.Customers.CountAsync();

        for (int i = 0; i < totalCustomers; i += batchSize)
        {
            var batch = await dbContext.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Skip(i)
                .Take(batchSize)
                .ToListAsync().ConfigureAwait(false);

            customersFromBatches.AddRange(batch);
        }

        // **No tracking**
        var customersNoTracking = await dbContext.Customers
            .AsNoTracking()
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);

        // ** Split queries **
        var customersSplitQueries = await dbContext.Customers
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync().ConfigureAwait(false);
    }
}