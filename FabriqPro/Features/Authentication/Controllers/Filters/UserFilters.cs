using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Authentication.Controllers.Filters;

public class UserFilters
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? Search { get; set; }
    public UserRoles? Role { get; set; }
}