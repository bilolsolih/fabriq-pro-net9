using FabriqPro.Features.Authentication.Controllers.Filters;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Authentication.Controllers;

[ApiController, Route("api/v1/users"), Authorize]
public class AuthController(AuthService service, TokenService tokenService) : ControllerBase
{
  [HttpPost("login"), AllowAnonymous]
  public async Task<ActionResult> Login(LoginDto payload)
  {
    var user = await service.GetUserByPhoneNumberAsync(payload.Login);
    if (user == null)
    {
      return Unauthorized("Bunday foydalanuvchi mavjud emas");
    }

    if (payload.Password == user.Password)
    {
      var token = tokenService.GenerateTokenAsync(user);
      return Ok(new { AccessToken = token });
    }

    return Unauthorized("Bunday foydalanuvchi mavjud emas");
  }

  [HttpPost("create")]
  public async Task<ActionResult<UserCreateDto>> CreateUser([FromForm] UserCreateDto payload)
  {
    var newUser = await service.CreateUserAsync(payload);
    return StatusCode(201, newUser);
  }

  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<UserListDto>>> ListUsers([FromQuery] UserFilters filters)
  {
    var users = await service.ListUsersAsync(filters);
    return Ok(users);
  }

  [HttpGet("retrieve/{id:int}")]
  public async Task<ActionResult<UserDetailDto>> RetrieveUser(int id)
  {
    var user = await service.RetrieveUserAsync(id);
    return Ok(user);
  }

  [HttpPatch("update/{id:int}")]
  public async Task<ActionResult<User>> UpdateUser(int id, [FromForm] UserUpdateDto payload)
  {
    var user = await service.UpdateUserAsync(id, payload);
    return Ok(user);
  }

  [HttpDelete("delete/{id:int}")]
  public async Task<ActionResult> DeleteUser(int id)
  {
    await service.DeleteUserAsync(id);
    return NoContent();
  }
}