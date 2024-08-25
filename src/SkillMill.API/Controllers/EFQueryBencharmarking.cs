using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using SkillMill.Application.Problems;

namespace SkillMill.API.Controllers;

public class EFQueryBencharmarking : BaseApiController
{
    [HttpGet("nplus-one")]
    public async Task<IActionResult> NPlusOneProblem()
    {
        var summary = BenchmarkRunner.Run<NPlusOneProblem>();

        return Ok();
    }

    [HttpGet("cartesian-explosion")]
    public async Task<IActionResult> CartesianExplosion()
    {
        var summary = BenchmarkRunner.Run<CartesianExplosionProblem>();

        return Ok();
    }

    [HttpGet("entity-tracking")]
    public async Task<IActionResult> EntityTracking()
    {
        var summary = BenchmarkRunner.Run<TrackingProblem>();

        return Ok();
    }
}