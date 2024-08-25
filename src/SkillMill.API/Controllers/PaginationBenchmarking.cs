using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using SkillMill.Application.Customers;

namespace SkillMill.API.Controllers;

public class PaginationBenchmarking : BaseApiController
{
    [HttpGet("customers/sieve-gridlify")]
    public async Task<IActionResult> BenchmarkSieveAndGridlify(CancellationToken cancellationToken)
    {
        var summary = BenchmarkRunner.Run<CustomerPaginationBenchmark>();

        return Ok(summary.Table.ToString());
    }
}