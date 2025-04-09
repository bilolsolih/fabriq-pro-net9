using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Features.Authentication.Controllers.Filters;
using FabriqPro.Features.Authentication.DTOs;
using FabriqPro.Features.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Authentication.Repositories;

public class AuthRepository(FabriqDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
{
    public async Task<User> AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<List<UserListDto>> GetAllAsync(UserFilters filters)
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

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        return user;
    }
    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }
    public async Task<User> UpdateAsync(User user)
    {
        user.Updated = DateTime.UtcNow;
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}