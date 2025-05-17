using FabriqPro.Features.Products.Controllers.Filters;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;
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

  public async Task<IEnumerable<ProductModel>> GetAllAsync(ProductModelFilters filters)
  {
    var query = context.ProductModels
      .Include(model => model.Color)
      .Include(model => model.ProductType)
      .AsQueryable();

    if (filters is { ProductTypeId: not null })
    {
      query = query.Where(p => p.ProductTypeId == filters.ProductTypeId);
    }

    if (filters is { Limit: not null, Page: not null })
    {
      query = query.Skip((int)((filters.Page - 1) * filters.Limit)).Take((int)filters.Limit);
    }

    return await query.ToListAsync();
  }

  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.ProductModels.AnyAsync(productModel => productModel.Id == id);
  }

  public async Task<bool> ExistsByTitleAsync(string title, int? colorId = null, int? productTypeId = null, bool caseSensitive = false)
  {
    if (caseSensitive)
      return await context.ProductModels.AnyAsync(productModel => productModel.Title == title);

    var query = context.ProductModels.Where(p => p.Title.ToLower() == title);

    if (colorId != null)
    {
      query = query.Where(p => p.ColorId == colorId);
    }

    if (productTypeId != null)
    {
      query = query.Where(p => p.ProductTypeId == productTypeId);
    }

    return await query.AnyAsync();
  }

  public async Task DeleteAsync(ProductModel productModel)
  {
    context.ProductModels.Remove(productModel);
    await context.SaveChangesAsync();
  }
}