using System.Security.Claims;
using FabriqPro.Features.Authentication.AuthorizationRequirements;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models;
using Microsoft.AspNetCore.Authorization;

namespace FabriqPro.Features.Authentication.AuthorizationHandlers;

public class UserRoleHandler : AuthorizationHandler<UserRoleRequirement>
{
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleRequirement requirement)
  {
    var claim = context.User.FindFirstValue("userRole");
    if (claim != null)
    {
      Enum.TryParse<UserRoles>(claim, out var userRole);
      if (requirement.UserRoles.Contains(userRole))
      {
        context.Succeed(requirement);
      }
    }

    return Task.CompletedTask;
  }
}