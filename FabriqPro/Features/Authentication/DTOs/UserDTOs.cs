using System.ComponentModel.DataAnnotations;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Authentication.DTOs;


public record LoginDto
{
    [MaxLength(32)]
    public required string Login { get; set; }

    [MaxLength(32)]
    public required string Password { get; set; }
}


public class UserCreateDto
{
    public required int DepartmentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PassportSeries { get; set; }
    public required DateOnly Birthdate { get; set; }
    public required string Address { get; set; }
    public required UserRoles Role { get; set; }
}

public class UserListDto
{
    public required int Id { get; set; }
    public string? ProfilePhoto { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required UserRoles Role { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
}

public class UserDetailDto
{
    public required int Id { get; set; }
    public required int DepartmentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required UserRoles Role { get; set; }
    public required string PassportSeries { get; set; }
    public required DateOnly Birthdate { get; set; }
    public string? ProfilePhoto { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }

    public double? Salary { get; set; }
    public double? WorkingHours { get; set; }
    public string? WorkingDays { get; set; }
}

public class UserUpdateDto
{
    public int? DepartmentId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PassportSeries { get; set; }
    public DateOnly? Birthdate { get; set; }
    public UserRoles? Role { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

    public double? Salary { get; set; }
    public double? WorkingHours { get; set; }
    public string? WorkingDays { get; set; }
}