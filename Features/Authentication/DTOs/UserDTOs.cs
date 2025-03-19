using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Authentication.DTOs;

public class UserCreateDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required UserRoles Role { get; set; }
}

public class UserListDto
{
    public required int Id { get; set; }
    public string? ProfilePhoto { get; set; }
    public required string FullName { get; set; }
    public required UserRoles Role { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
}