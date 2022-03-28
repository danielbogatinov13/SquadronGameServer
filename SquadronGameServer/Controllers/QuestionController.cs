using Microsoft.AspNetCore.Mvc;

namespace SquadronGameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}
