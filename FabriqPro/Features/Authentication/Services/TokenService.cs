using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FabriqPro.Features.Authentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace FabriqPro.Features.Authentication.Services;

public class TokenService(IConfiguration config)
{
    public string GenerateTokenAsync( int id, UserRoles userRole, string login)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        var secret = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);
        var claims = new[]
        {
            new Claim("id", id.ToString()),
            new Claim("login", login),
            new Claim("role", userRole.ToString()),
            new Claim("expiryDate", DateTime.UtcNow.AddHours(6).ToString("O")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(secret);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddYears(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}