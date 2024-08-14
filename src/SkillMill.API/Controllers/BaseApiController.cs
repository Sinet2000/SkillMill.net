using Microsoft.AspNetCore.Mvc;

namespace SkillMill.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
}