using FabriqPro.Features.Products.Repositories;
using FabriqPro.Features.Products.Services;

namespace FabriqPro.Features.Products;

public static class Extensions
{
  public static void RegisterProductsFeature(this IServiceCollection services)
  {
    services.AddScoped<ColorRepository>();
    services.AddScoped<ColorService>();
    
    services.AddScoped<DepartmentRepository>();
    services.AddScoped<DepartmentService>();
    
    services.AddScoped<ProductTypeRepository>();
    services.AddScoped<ProductTypeService>();
        
    services.AddScoped<MaterialTypeRepository>();
    services.AddScoped<MaterialTypeService>();
    
    services.AddScoped<ProductModelRepository>();
    services.AddScoped<ProductModelService>();
  }
}