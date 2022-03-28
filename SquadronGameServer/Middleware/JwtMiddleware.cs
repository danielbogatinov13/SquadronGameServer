using GameServer.Services.AppSettings;
using GameServer.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JWT _jwt;

    public JwtMiddleware(RequestDelegate next, IOptions<JWT> jwt)
    {
        _next = next;
        _jwt = jwt.Value;
    }

    public async Task Invoke(HttpContext context, IAccountService accountService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContext(context, accountService, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IAccountService accountService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var username = jwtToken.Claims.First(x => x.Type == "username").Value;

            // attach user to context on successful jwt validation
            context.Items["User"] = await accountService.GetByUsername(username);
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}