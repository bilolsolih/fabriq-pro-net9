using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Features.Authentication.Controllers.Filters;
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
    public async Task<ActionResult<IEnumerable<UserListDto>>> ListUsers([FromQuery] UserFilters filters)
    {
        var baseUrl = HttpContext.GetUploadsBaseUrl();
        var usersQuery = context.Users.AsQueryable();

        if (filters is { Search: not null })
        {
            filters.Search = filters.Search.ToLower();
            usersQuery = usersQuery.Where(user =>
                user.FirstName.ToLower().Contains(filters.Search) ||
                user.LastName.ToLower().Contains(filters.Search) ||
                user.PhoneNumber.Contains(filters.Search)
            );
        }

        if (filters is { Page: not null, Limit: not null })
        {
            usersQuery = usersQuery.Skip((int)(filters.Limit * (filters.Page - 1)));
            var totalCount = Math.Ceiling((double)(usersQuery.Count() / filters.Page));
            HttpContext.Response.Headers.Append("totalPages", $"{totalCount}");
        }

        if (filters is { Limit: not null })
        {
            usersQuery = usersQuery.Take((int)filters.Limit);
        }

        var users = await usersQuery.ProjectTo<UserListDto>(mapper.ConfigurationProvider).ToListAsync();
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