using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Repositories;

public class ProductTypeRepository(FabriqDbContext context)
{
  public async Task<ProductType> AddAsync(ProductType productType)
  {
    context.ProductTypes.Add(productType);
    await context.SaveChangesAsync();
    return productType;
  }

  public async Task<ProductType> UpdateAsync(ProductType productType)
  {
    productType.Updated = DateTime.UtcNow;
    context.ProductTypes.Update(productType);
    await context.SaveChangesAsync();
    return productType;
  }

  public async Task<ProductType?> GetByIdAsync(int productTypeId)
  {
    return await context.ProductTypes.FindAsync(productTypeId);
  }

  public async Task<IEnumerable<ProductType>> GetAllAsync()
  {
    return await context.ProductTypes.ToListAsync();
  }

  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.ProductTypes.AnyAsync(productType => productType.Id == id);
  }

  public async Task<bool> ExistsByTitleAsync(string title, bool caseSensitive = false)
  {
    if (caseSensitive)
      return await context.ProductTypes.AnyAsync(productType => productType.Title == title);

    return await context.ProductTypes.AnyAsync(productType => productType.Title.ToLower() == title.ToLower());
  }

  public async Task DeleteAsync(ProductType productType)
  {
    context.ProductTypes.Remove(productType);
    await context.SaveChangesAsync();
  }
}