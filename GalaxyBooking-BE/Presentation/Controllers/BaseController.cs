using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        protected virtual IActionResult InternalServerError(string reason)
        {
            var result = new ObjectResult(new { message = $"{reason}" })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return result;
        }
    }
}
