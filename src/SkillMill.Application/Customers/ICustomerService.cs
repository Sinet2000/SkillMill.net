using Sieve.Models;
using SkillMill.Application.Customers.Dtos;
using SkillMill.Data.Common.Models;

namespace SkillMill.Application.Customers;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> List(SieveModel sieveModel, CancellationToken cancellationToken);
}