using FabriqPro.Features.Authentication.Configurations;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Clients.Configurations;
using FabriqPro.Features.Clients.Models;
using FabriqPro.Features.Products.Configurations;
using FabriqPro.Features.Products.Configurations.MaterialConfigs;
using FabriqPro.Features.Products.Configurations.ProductConfigs;
using FabriqPro.Features.Products.Configurations.ProductPartConfigs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Accessory;
using FabriqPro.Features.Products.Models.Material;
using FabriqPro.Features.Products.Models.Miscellaneous;
using FabriqPro.Features.Products.Models.Product;
using FabriqPro.Features.Products.Models.ProductPart;
using FabriqPro.Features.Products.Models.SparePart;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro;

public class FabriqDbContext(DbContextOptions<FabriqDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }

  public DbSet<Client> Clients { get; set; }

  public DbSet<Color> Colors { get; set; }
  public DbSet<ProductType> ProductTypes { get; set; }
  public DbSet<ProductModel> ProductModels { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<ProductPartType> ProductPartTypes { get; set; }
  public DbSet<ProductPart> ProductParts { get; set; }

  public DbSet<MaterialType> MaterialTypes { get; set; }
  public DbSet<Material> Materials { get; set; }
  public DbSet<Party> Parties { get; set; }
  public DbSet<Cutting> Cuttings { get; set; }

  public DbSet<AccessoryType> AccessoryTypes { get; set; }
  public DbSet<Accessory> Accessories { get; set; }

  public DbSet<SparePartType> SparePartTypes { get; set; }
  public DbSet<SparePart> SpareParts { get; set; }

  public DbSet<MiscellaneousType> MiscellaneousTypes { get; set; }
  public DbSet<Miscellaneous> Miscellaneous { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.ApplyConfiguration(new UserConfigurations());

    builder.ApplyConfiguration(new ClientConfigurations());

    builder.ApplyConfiguration(new ColorConfigurations());
    builder.ApplyConfiguration(new ProductTypeConfigurations());
    builder.ApplyConfiguration(new ProductModelConfigurations());
    builder.ApplyConfiguration(new ProductConfigurations());

    builder.ApplyConfiguration(new ProductPartTypeConfigurations());
    builder.ApplyConfiguration(new ProductPartConfigurations());
    
    builder.ApplyConfiguration(new MaterialTypeConfigurations());
    builder.ApplyConfiguration(new MaterialConfigurations());
    builder.ApplyConfiguration(new PartyConfigurations());
    builder.ApplyConfiguration(new CuttingConfigurations());

    builder.ApplyConfiguration(new AccessoryTypeConfigurations());
    builder.ApplyConfiguration(new AccessoryConfigurations());

    builder.ApplyConfiguration(new SparePartTypeConfigurations());
    builder.ApplyConfiguration(new SparePartConfigurations());

    builder.ApplyConfiguration(new MiscellaneousTypeConfigurations());
    builder.ApplyConfiguration(new MiscellaneousConfigurations());
  }
}