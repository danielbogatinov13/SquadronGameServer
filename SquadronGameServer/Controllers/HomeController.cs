using Microsoft.AspNetCore.Mvc;

namespace SquadronGameServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}
