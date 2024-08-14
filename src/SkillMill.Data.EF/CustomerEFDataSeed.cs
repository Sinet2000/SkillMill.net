using SkillMill.Data.Common;
using SkillMill.Data.Common.Fakers;

namespace SkillMill.Data.EF;

public class CustomerEFDataSeed(AppDbContext dbContext, CustomerFaker faker) : ISeedApplier
{
    private const int CustomersCountToGenerate = 300;

    public async Task ApplyAsync()
    {
        if (dbContext.Customers.Any())
        {
            return;
        }

        var customers = faker.Generate(CustomersCountToGenerate).ToList();
        await dbContext.Customers.AddRangeAsync(customers);
        await dbContext.SaveChangesAsync();
    }
}