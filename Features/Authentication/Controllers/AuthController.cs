using FabriqPro.Features.Authentication.Controllers.Filters;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Authentication.Controllers;

[ApiController, Route("api/v1/users")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<UserCreateDto>> CreateUser([FromForm] UserCreateDto payload)
    {
        var newUser = await authService.CreateUserAsync(payload);
        return StatusCode(201, newUser);
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<UserListDto>>> ListUsers([FromQuery] UserFilters filters)
    {
        var users = await authService.ListUsersAsync(filters);
        return Ok(users);
    }

    [HttpGet("retrieve/{id:int}")]
    public async Task<ActionResult<UserDetailDto>> RetrieveUser(int id)
    {
        var user = await authService.RetrieveUserAsync(id);
        return Ok(user);
    }

    [HttpPatch("update/{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromForm] UserUpdateDto payload)
    {
        var user = await authService.UpdateUserAsync(id, payload);
        return Ok(user);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await authService.DeleteUserAsync(id);
        return NoContent();
    }
}