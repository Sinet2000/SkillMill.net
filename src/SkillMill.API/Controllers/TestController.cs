using Microsoft.AspNetCore.Mvc;

namespace SkillMill.API.Controllers;

public class TestController : BaseApiController
{
    [HttpGet]
    public Task<IActionResult> TestApp()
    {
        int[] numbers = Enumerable.Range(1, 1000000).ToArray();
        long sumOfSquares = 0;
        Parallel.For(0, numbers.Length, () => 0L, (i, state, localSum) =>
            {
                localSum += (long)numbers[i] * numbers[i];

                return localSum;
            },
            localSum =>
            {
                lock (numbers)
                {
                    sumOfSquares += localSum;
                }
            });

        return Task.FromResult<IActionResult>(Ok(new { Sum = sumOfSquares }));
    }
}