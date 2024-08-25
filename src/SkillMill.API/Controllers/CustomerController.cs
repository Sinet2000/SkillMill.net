using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using SkillMill.Application.Customers;

namespace SkillMill.API.Controllers;

public class CustomerController(ICustomerService service) : BaseApiController
{
    /// <summary>
    /// Example:  ?filters=Name@=a&page=1&pageSize=10
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ListPaged([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken)
    {
        return Ok(await service.List(sieveModel, cancellationToken));
    }
}