using FabriqPro.Features.Authentication.Models;
using Microsoft.AspNetCore.Authorization;

namespace FabriqPro.Features.Authentication.AuthorizationRequirements;

public class UserRoleRequirement(List<UserRoles> roles) : IAuthorizationRequirement
{
  public List<UserRoles> UserRoles { get; } = roles;
}