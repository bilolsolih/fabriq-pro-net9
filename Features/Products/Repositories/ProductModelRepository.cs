using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Repositories;

public class ProductModelRepository(FabriqDbContext context)
{
  public async Task<ProductModel> AddAsync(ProductModel productModel)
  {
    context.ProductModels.Add(productModel);
    await context.SaveChangesAsync();
    return productModel;
  }

  public async Task<ProductModel> UpdateAsync(ProductModel productModel)
  {
    productModel.Updated = DateTime.UtcNow;
    context.ProductModels.Update(productModel);
    await context.SaveChangesAsync();
    return productModel;
  }

  public async Task<ProductModel?> GetByIdAsync(int productModelId)
  {
    return await context.ProductModels
      .Include(model => model.Color)
      .Include(model => model.ProductType)
      .Where(model => model.Id == productModelId)
      .SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<ProductModel>> GetAllAsync()
  {
    return await context.ProductModels
      .Include(model => model.Color)
      .Include(model => model.ProductType)
      .ToListAsync();
  }

  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.ProductModels.AnyAsync(productModel => productModel.Id == id);
  }

  public async Task<bool> ExistsByTitleAsync(string title, bool caseSensitive = false)
  {
    if (caseSensitive)
      return await context.ProductModels.AnyAsync(productModel => productModel.Title == title);

    return await context.ProductModels.AnyAsync(productModel => productModel.Title.ToLower() == title.ToLower());
  }

  public async Task DeleteAsync(ProductModel productModel)
  {
    context.ProductModels.Remove(productModel);
    await context.SaveChangesAsync();
  }
}