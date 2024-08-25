using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using SkillMill.Application.Customers;

namespace SkillMill.API.Controllers;

public class MappingsBenchmark : BaseApiController
{
    [HttpGet]
    public Task<IActionResult> Customers()
    {
        BenchmarkRunner.Run<CustomerMappingBenchmark>();

        return Task.FromResult<IActionResult>(Ok());
    }
}