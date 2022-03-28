using GameServer.Models;
using GameServer.Services.Dtos.Round;
using GameServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SquadronGameServer.SignalR;

namespace SquadronGameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private IHubContext<RoundHub> _hub;
        private readonly IGameService _gameService;
        private readonly IRoundService _roundService;

        public RoundController(IHubContext<RoundHub> hub,
            IGameService gameService,
            IRoundService roundService)
        {
            _hub = hub;
            _gameService = gameService;
            _roundService = roundService;
        }

        [HttpPut]
        public async Task<IActionResult> Put(int roundId, [FromBody] RoundDto roundDto)
        {
            try
            {
                var user = (User)ControllerContext.HttpContext.Items["User"];
                var answeredRound = _roundService.AnswerRound(roundDto, user.Id);
                answeredRound.IsAnswered = true;
                if (!answeredRound.IsAnswerCorrect)
                {
                    return Ok(answeredRound);
                }
                var activePlayers = _gameService.GetPlayers();
                var round = _roundService.CreateRound();
                await _hub.Clients.All.SendAsync("nextRound", round);
                return Ok(answeredRound);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
