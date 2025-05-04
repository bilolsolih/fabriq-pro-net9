using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FabriqPro.Features.Authentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace FabriqPro.Features.Authentication.Services;

public class TokenService(IConfiguration config)
{
  public string GenerateTokenAsync(User user)
  {
    var jwtSettings = config.GetSection("JwtSettings");
    var secret = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);
    var claims = new[]
    {
      new Claim("id", user.Id.ToString()),
      new Claim("login", user.PhoneNumber),
      new Claim("userRole", user.Role.ToString()),
      new Claim("department", user.Department.ToString()),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

    var key = new SymmetricSecurityKey(secret);
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.Now.AddHours(6),
      signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}