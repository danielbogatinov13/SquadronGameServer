using GameServer.Services.Dtos.Account;
using GameServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SquadronGameServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService
            )
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _accountService.Login(loginDto);
                if (user is null)
                {
                    return Unauthorized();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                await _accountService.Register(registerDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
