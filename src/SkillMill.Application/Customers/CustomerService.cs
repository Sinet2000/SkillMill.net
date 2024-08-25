using Sieve.Models;
using SkillMill.Application.Customers.Dtos;
using SkillMill.Data.Common.Models;
using SkillMill.Data.EF.Interfaces;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers;

public class CustomerService(IDataSearchQuery<AppDbContext> searchCtx) : ICustomerService
{
    public async Task<PagedResult<CustomerDto>> List(SieveModel sieveModel, CancellationToken cancellationToken)
    {
        return await searchCtx.ListAsync<Customer, CustomerDto>(sieveModel, cancellationToken: cancellationToken);
    }
}