using GameServer.Models;
using GameServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SquadronGameServer.Attributes;

namespace SquadronGameServer.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IRoundService _roundService;
        public GameController(
            IGameService gameService,
            IRoundService roundService)
        {
            _gameService = gameService;
            _roundService = roundService;
        }

        [HttpGet]
        public async Task<IActionResult> EnterGame()
        {
            try
            {
                var user = (User)ControllerContext.HttpContext.Items["User"];
                var gameId = _gameService.EnterGame(user.Id);
                var round = _roundService.GetActiveRound();
                round.GameId = gameId;
                return Ok(round);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExitGame()
        {
            try
            {
                var user = (User)ControllerContext.HttpContext.Items["User"];
                _gameService.ExitGame(user.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
