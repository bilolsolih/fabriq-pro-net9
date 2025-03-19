using FabriqPro.Features.Authentication.Configurations;
using FabriqPro.Features.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro;

public class FabriqDbContext(DbContextOptions<FabriqDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfigurations());
    }
}