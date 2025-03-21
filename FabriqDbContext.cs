using FabriqPro.Features.Authentication.Configurations;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Clients.Configurations;
using FabriqPro.Features.Clients.Models;
using FabriqPro.Features.Products.Configurations;
using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro;

public class FabriqDbContext(DbContextOptions<FabriqDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }

    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductModel> ProductModels { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductToDepartment> ProductsToDepartments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfigurations());
        builder.ApplyConfiguration(new ClientConfigurations());
        builder.ApplyConfiguration(new ColorConfigurations());
        builder.ApplyConfiguration(new ProductTypeConfigurations());
        builder.ApplyConfiguration(new ProductModelConfigurations());
        builder.ApplyConfiguration(new DepartmentConfigurations());
        builder.ApplyConfiguration(new ProductConfigurations());
        builder.ApplyConfiguration(new ProductToDepartmentConfigurations());
    }
}