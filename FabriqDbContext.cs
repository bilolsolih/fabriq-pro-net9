using Microsoft.EntityFrameworkCore;

namespace FabriqPro;

public class FabriqDbContext(DbContextOptions<FabriqDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}