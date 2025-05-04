using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FabriqPro.Tests;

public class ApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
  private FabriqDbContext? _context;

  private void SeedDatabaseWithUsers()
  {
    List<User> users =
    [
      new()
      {
        PhoneNumber = "+998900000000",
        FirstName = "Super",
        LastName = "Admin",
        Department = Department.Storage,
        Address = "Address",
        Birthdate = DateOnly.Parse("2000-01-01"),
        PassportSeries = "AA0000000",
        Role = UserRoles.SuperAdmin,
        Password = "superadmin",
      },
      new()
      {
        PhoneNumber = "+998900000001",
        FirstName = "Storage",
        LastName = "Master",
        Department = Department.Storage,
        Address = "Address",
        Birthdate = DateOnly.Parse("2000-01-01"),
        PassportSeries = "AA0000001",
        Role = UserRoles.StorageManager,
        Password = "storagemaster",
      },
    ];
    _context.Users.AddRange(users);
    

    List<Color> colors =
    [
      new() { Title = "Oq", ColorCode = "#FFFFFF" },
      new() { Title = "Qora", ColorCode = "#000000" },
    ];
    _context.Colors.AddRange(colors);

    List<Material> materials = [new() { Title = "XB" }, new() { Title = "Sintetika" }];
    _context.Materials.AddRange(materials);

    List<Party> parties =
    [
      new() { Title = "AA-0001" },
      new() { Title = "AA-0002" },
      new() { Title = "AA-0003" }, 
      new() { Title = "AA-0004" }
    ];
    _context.Parties.AddRange(parties);
    _context.SaveChanges();

    List<MaterialToDepartment> materialDepartments =
    [
      new()
      {
        MaterialId = 1,
        ColorId = 1,
        PartyId = 1,
        UserId = 1,
        Unit = Unit.Kg,
        Quantity = 100,
        Department = Department.Storage,
        HasPatterns = true,
        Thickness = 100,
        Width = 100,
      },
      new()
      {
        MaterialId = 1,
        ColorId = 2,
        PartyId = 2,
        UserId = 2,
        Unit = Unit.Kg,
        Quantity = 400,
        Department = Department.Storage,
        HasPatterns = false,
        Thickness = 100,
        Width = 100,
      },
      new()
      {
        MaterialId = 2,
        ColorId = 1,
        PartyId = 3,
        UserId = 1,
        Unit = Unit.Meter,
        Quantity = 400,
        Department = Department.Storage,
        HasPatterns = true,
        Thickness = 100,
        Width = 100,
      },
      new()
      {
        MaterialId = 2,
        ColorId = 2,
        PartyId = 4,
        UserId = 2,
        Unit = Unit.Meter,
        Quantity = 400,
        Department = Department.Storage,
        HasPatterns = false,
        Thickness = 100,
        Width = 100,
      },
    ];
    _context.MaterialInDepartments.AddRange(materialDepartments);
    _context.SaveChanges();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
      {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FabriqDbContext>));

        if (descriptor != null)
        {
          services.Remove(descriptor);
        }

        services.AddNpgsql<FabriqDbContext>(
          "Host=localhost;Username=postgres;Database=fabriq_db_test;Port=5432;Password=123;Include Error Detail=true"
        );

        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<FabriqDbContext>();
        _context.Database.EnsureCreated();
        SeedDatabaseWithUsers();
      }
    );
    base.ConfigureWebHost(builder);
  }

  public new void Dispose()
  {
    
    base.Dispose();
  }
}