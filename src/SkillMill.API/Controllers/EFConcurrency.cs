using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkillMill.API.Configuration;
using SkillMill.Data.EF;
using SkillMill.Domain.Entities;

namespace SkillMill.API.Controllers;

public class EFConcurrency(IOptions<CoreAppConfiguration> appConfigOptions) : BaseApiController
{
    private readonly CoreAppConfiguration _appConfiguration = appConfigOptions.Value;

    [HttpGet("orders/concurrent-update")]
    public async Task<IActionResult> CustomerUpdateErrorInParallelExample()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(_appConfiguration.DbConnectionString);
        using (var context = new AppDbContext(optionsBuilder.Options))
        {
            var randomCustomer = await context.Customers.OrderBy(o => Guid.NewGuid()).FirstOrDefaultAsync().ConfigureAwait(false);

            if (randomCustomer == null) return NotFound("No orders found.");

            var tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (var localContext = new AppDbContext(optionsBuilder.Options))
                    {
                        try
                        {
                            var order = await localContext.Customers.SingleAsync(o => o.Id == randomCustomer.Id).ConfigureAwait(false);
                            Thread.Sleep(100);
                            order.UpdateEmail(new Faker().Internet.Email());
                            await localContext.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            foreach (var entry in ex.Entries)
                            {
                                if (entry.Entity is Customer)
                                {
                                    var proposedValues = entry.CurrentValues;
                                    var databaseValues = await entry.GetDatabaseValuesAsync();

                                    foreach (var property in proposedValues.Properties)
                                    {
                                        var proposedValue = proposedValues[property];

                                        proposedValues[property] = proposedValue;
                                    }

                                    entry.OriginalValues.SetValues(databaseValues);
                                }
                                else
                                {
                                    throw new NotSupportedException(
                                        "Don't know how to handle concurrency conflicts for "
                                        + entry.Metadata.Name);
                                }
                            }
                        }
                    }
                }));
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

            return Ok();
        }
    }
}