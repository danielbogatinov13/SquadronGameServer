using AutoMapper;
using GameServer.Models;
using GameServer.Services.AppSettings;
using GameServer.Services.Dtos.Account;
using GameServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameServer.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwtSettings;
        private readonly IMapper _mapper;

        public AccountService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwtSettings,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userDto = _mapper.Map<UserDto>(user);

                userDto.Token = GetToken(user);
                return userDto;
            }
            return null;
        }

        public async Task Register(RegisterDto loginDto)
        {
            var userExists = await _userManager.FindByNameAsync(loginDto.Username);
            if (userExists != null)
            {
                throw new Exception("Username alredy exists!");
            }
            User user = new()
            {
                Email = loginDto.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = loginDto.Username,
                FirstName = loginDto.FirstName,
                LastName = loginDto.LastName
            };
            var roleExists = await _roleManager.FindByNameAsync("Player");
            if (roleExists is null)
            {
                IdentityRole role = new()
                {
                    Name = "Player"
                };
                var roleResult = await _roleManager.CreateAsync(role);
            }
            var result = await _userManager.CreateAsync(user, loginDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed! Please check user details and try again.");
            }
            var roleAddingResult = await _userManager.AddToRoleAsync(user, "Player");
            if (!roleAddingResult.Succeeded)
            {
                throw new Exception("User Role adding failed! Please check user details and try again.");
            }
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        private string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id), new Claim("username", user.UserName) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
