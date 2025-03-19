using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Authentication.Controllers;

[ApiController, Route("api/v1/users")]
public class AuthController(FabriqDbContext context, IMapper mapper, IWebHostEnvironment webEnv) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<UserCreateDto>> CreateUser(UserCreateDto payload)
    {
        var newUser = mapper.Map<User>(payload);

        if (payload.ProfilePhoto != null)
        {
            var usersDir = Path.Combine(webEnv.ContentRootPath, "uploads", "users");
            if (!Directory.Exists(usersDir))
            {
                Directory.CreateDirectory(usersDir);
            }

            var fileName = GenerateFileName(payload);

            var filePath = Path.Combine(usersDir, fileName);
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await payload.ProfilePhoto.CopyToAsync(fileStream);
            newUser.ProfilePhoto = $"/users/{fileName}";
        }


        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return StatusCode(201, payload);
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<UserListDto>>> ListUsers()
    {
        var baseUrl = HttpContext.GetUploadsBaseUrl();
        var users = await context.Users.ProjectTo<UserListDto>(mapper.ConfigurationProvider).ToListAsync();
        users.ForEach(user =>
        {
            if (user.ProfilePhoto != null)
            {
                user.ProfilePhoto = $"{baseUrl}{user.ProfilePhoto}";
            }
        });
        return Ok(users);
    }

    private string GenerateFileName(UserCreateDto payload)
    {
        var fileName = new StringBuilder();
        fileName.Append($"{payload.FirstName}_{payload.LastName}_{payload.PhoneNumber}");
        fileName.Append($".{payload.ProfilePhoto!.FileName.Split('.').Last()}");
        fileName = fileName.Replace(" ", string.Empty);
        return fileName.ToString();
    }
}