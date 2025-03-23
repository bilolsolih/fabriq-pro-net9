using FabriqPro.Features.Products.Repositories;
using FabriqPro.Features.Products.Services;

namespace FabriqPro.Features.Products;

public static class Extensions
{
  public static void RegisterProductsFeature(this IServiceCollection services)
  {
    services.AddScoped<ColorRepository>();
    services.AddScoped<ColorService>();
  }
}