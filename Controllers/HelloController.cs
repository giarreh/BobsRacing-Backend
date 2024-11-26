using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello, World!");
        }
    }
}
