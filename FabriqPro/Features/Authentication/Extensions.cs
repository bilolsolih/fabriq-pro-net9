using System.Text;
using FabriqPro.Features.Authentication.AuthorizationHandlers;
using FabriqPro.Features.Authentication.AuthorizationRequirements;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Authentication.Repositories;
using FabriqPro.Features.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FabriqPro.Features.Authentication;

public static class Extensions
{
  public static void RegisterAuthenticationFeature(this IServiceCollection services, ConfigurationManager config)
  {
    services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }
    ).AddJwtBearer(options =>
      {
        var jwtSettings = config.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateLifetime = true,
          RequireAudience = false,
          ValidateIssuer = false,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
          ClockSkew = TimeSpan.Zero
        };
      }
    );

    services.AddAuthorization(options =>
      {
        options.AddPolicy(
          "SuperAdmin",
          policy => policy.Requirements.Add(new UserRoleRequirement([UserRoles.SuperAdmin]))
        );

        options.AddPolicy(
          "Master",
          policy => policy.Requirements.Add(
            new UserRoleRequirement([UserRoles.CuttingMaster, UserRoles.SewingMaster, UserRoles.PackagingMaster])
          )
        );

        options.AddPolicy(
          "StorageManagerOrSuperAdmin",
          policy => policy.Requirements.Add(new UserRoleRequirement([UserRoles.SuperAdmin, UserRoles.StorageManager]))
        );

        options.AddPolicy(
          "CuttingMaster",
          policy => policy.Requirements.Add(new UserRoleRequirement([UserRoles.CuttingMaster]))
        );

        options.AddPolicy(
          "CuttingMasterOrSuperAdmin",
          policy => policy.Requirements.Add(
            new UserRoleRequirement([UserRoles.SuperAdmin, UserRoles.CuttingMaster])
          )
        );

        options.AddPolicy(
          "SewingMasterOrSuperAdmin",
          policy => policy.Requirements.Add(
            new UserRoleRequirement([UserRoles.SuperAdmin, UserRoles.SewingMaster])
          )
        );

        options.AddPolicy(
          "PackagingMasterOrStorageManagerOrSuperAdmin",
          policy => policy.Requirements.Add(
            new UserRoleRequirement([UserRoles.SuperAdmin, UserRoles.PackagingMaster, UserRoles.StorageManager])
          )
        );
      }
    );
    services.AddSingleton<IAuthorizationHandler, UserRoleHandler>();
    services.AddScoped<AuthService>();
    services.AddScoped<TokenService>();
    services.AddScoped<AuthRepository>();
  }
}