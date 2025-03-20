using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Features.Authentication.Controllers.Filters;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Authentication.Repositories;

public class AuthRepository(FabriqDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
{
    public async Task<User> CreateUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<List<UserListDto>> ListUsersAsync(UserFilters filters)
    {
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
            var totalCount = Math.Ceiling(usersQuery.Count() / (double)filters.Limit);
            httpContextAccessor.HttpContext!.Response.Headers.Append("totalPages", $"{totalCount}");
            usersQuery = usersQuery.Skip((int)(filters.Limit * (filters.Page - 1)));
        }

        if (filters is { Limit: not null })
        {
            usersQuery = usersQuery.Take((int)filters.Limit);
        }

        var users = await usersQuery.ProjectTo<UserListDto>(mapper.ConfigurationProvider).ToListAsync();
        return users;
    }

    public async Task<UserDetailDto> GetUserDetailAsync(int id)
    {
        var user = await context.Users.ProjectTo<UserDetailDto>(mapper.ConfigurationProvider)
            .SingleAsync(user => user.Id == id);
        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        return user;
    }

    public async Task UpdateUserAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}