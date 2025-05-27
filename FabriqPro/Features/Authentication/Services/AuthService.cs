using System.Text;
using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Controllers.Filters;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Authentication.Repositories;

namespace FabriqPro.Features.Authentication.Services;

public class AuthService(
  AuthRepository authRepo,
  IWebHostEnvironment webEnv,
  IMapper mapper,
  IHttpContextAccessor httpContextAccessor
)
{
  public async Task<User> CreateUserAsync(UserCreateDto payload)
  {
    var user = mapper.Map<User>(payload);

    if (payload.ProfilePhoto != null)
    {
      var fileName = GenerateFileName(
        firstName: payload.FirstName,
        lastName: payload.LastName,
        phoneNumber: payload.PhoneNumber,
        fileName: payload.ProfilePhoto.FileName
      );

      user.ProfilePhoto = await SaveUploadFile(payload.ProfilePhoto, fileName, "users");
    }

    var newUser = await authRepo.AddAsync(user);

    return newUser;
  }

  public async Task<List<UserListDto>> ListUsersAsync(UserFilters filters)
  {
    ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);

    var baseUrl = httpContextAccessor.HttpContext.GetUploadsBaseUrl();

    var users = await authRepo.GetAllAsync(filters);

    users.ForEach(
      user =>
      {
        if (user.ProfilePhoto != null)
        {
          user.ProfilePhoto = $"{baseUrl}/{user.ProfilePhoto}";
        }
      }
    );
    return users;
  }

  public async Task<UserDetailDto> RetrieveUserAsync(int id)
  {
    ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);
    var baseUrl = httpContextAccessor.HttpContext.GetUploadsBaseUrl();

    var user = await authRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(user, nameof(user));

    if (user.ProfilePhoto != null)
    {
      user.ProfilePhoto = $"{baseUrl}/{user.ProfilePhoto}";
    }

    return mapper.Map<UserDetailDto>(user);
  }
  
  public async Task<User?> GetUserByPhoneNumberAsync(string value)
  {
    var user = await authRepo.GetByPhoneNumberAsync(value);
    return user;
  }

  public async Task<User> UpdateUserAsync(int id, UserUpdateDto payload)
  {
    var user = await authRepo.GetByIdAsync(id);
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

    user = await authRepo.UpdateAsync(user);
    user.Updated = user.Updated.ToLocalTime();
    user.Created = user.Created.ToLocalTime();
    return user;
  }

  public async Task DeleteUserAsync(int id)
  {
    var user = await authRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(user, nameof(User));

    if (user.ProfilePhoto != null)
    {
      DeleteUploadFile(user.ProfilePhoto);
    }

    await authRepo.DeleteAsync(user);
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

  private void DeleteUploadFile(string fileName)
  {
    var oldPhotoPath = Path.Combine(webEnv.ContentRootPath, "uploads", fileName);
    var fileInfo = new FileInfo(oldPhotoPath);
    if (fileInfo.Exists)
    {
      fileInfo.Delete();
    }
  }
}