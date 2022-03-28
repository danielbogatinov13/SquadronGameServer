using GameServer.Models;
using GameServer.Services.Dtos.Account;

namespace GameServer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task Register(RegisterDto loginDto);
        Task<User> GetByUsername(string username);
    }
}
