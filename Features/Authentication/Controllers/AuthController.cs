using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
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
            var fileName = GenerateFileName(
                firstName: payload.FirstName,
                lastName: payload.LastName,
                phoneNumber: payload.PhoneNumber,
                fileName: payload.ProfilePhoto.FileName
            );

            newUser.ProfilePhoto = await SaveUploadFile(payload.ProfilePhoto, fileName, "users");
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
            usersQuery = usersQuery.Where(
                user =>
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
        users.ForEach(
            user =>
            {
                if (user.ProfilePhoto != null)
                {
                    user.ProfilePhoto = $"{baseUrl}/{user.ProfilePhoto}";
                }
            }
        );
        return Ok(users);
    }

    [HttpGet("retrieve/{id:int}")]
    public async Task<ActionResult<UserDetailDto>> RetrieveUser(int id)
    {
        var user = await context.Users.ProjectTo<UserDetailDto>(mapper.ConfigurationProvider)
            .SingleAsync(user => user.Id == id);

        if (user.ProfilePhoto != null)
        {
            var baseUrl = HttpContext.GetUploadsBaseUrl();
            user.ProfilePhoto = $"{baseUrl}/{user.ProfilePhoto}";
        }

        return Ok(user);
    }

    [HttpPatch("update/{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromForm] UserUpdateDto payload)
    {
        var user = await context.Users.FindAsync(id);
        DoesNotExistException.ThrowIfNull(user, nameof(User));
        if (payload.ProfilePhoto != null && user.ProfilePhoto != null)
        {
            DeleteUploadFile(user.ProfilePhoto);
        }

        mapper.Map(payload, user);

        if (payload.ProfilePhoto != null)
        {
            var fileName = GenerateFileName(
                firstName: payload.FirstName ?? user.FirstName,
                lastName: payload.LastName ?? user.LastName,
                phoneNumber: payload.PhoneNumber ?? user.PhoneNumber,
                fileName: payload.ProfilePhoto.FileName
            );
            user.ProfilePhoto = await SaveUploadFile(payload.ProfilePhoto, fileName, "users");
        }

        await context.SaveChangesAsync();
        user.Updated = user.Updated.ToLocalTime();
        user.Created = user.Created.ToLocalTime();
        return Ok(user);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        DoesNotExistException.ThrowIfNull(user, nameof(User));
        if (user.ProfilePhoto != null)
        {
            DeleteUploadFile(user.ProfilePhoto);
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private string GenerateFileName(string firstName, string lastName, string phoneNumber, string fileName)
    {
        var fileNameBuilder = new StringBuilder();
        fileNameBuilder.Append($"{firstName}_{lastName}_{phoneNumber}");
        fileNameBuilder.Append($".{fileName.Split('.').Last()}");
        fileNameBuilder = fileNameBuilder.Replace(" ", string.Empty);
        return fileNameBuilder.ToString();
    }

    private async Task<string> SaveUploadFile(IFormFile file, string fileName, string uploadDirName)
    {
        var uploadDirPath = Path.Combine(webEnv.ContentRootPath, "uploads", uploadDirName);
        if (!Directory.Exists(uploadDirPath))
        {
            Directory.CreateDirectory(uploadDirPath);
        }

        var filePath = Path.Combine(uploadDirPath, fileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return $"users/{fileName}";
    }

    private bool DeleteUploadFile(string fileName)
    {
        var oldPhotoPath = Path.Combine(webEnv.ContentRootPath, "uploads", fileName);
        var info = new FileInfo(oldPhotoPath);

        if (!info.Exists) return false;
        info.Delete();
        return true;
    }
}