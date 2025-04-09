using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Repositories;

public class MaterialTypeRepository(FabriqDbContext context)
{
  public async Task<MaterialType> AddAsync(MaterialType materialType)
  {
    context.MaterialTypes.Add(materialType);
    await context.SaveChangesAsync();
    return materialType;
  }

  public async Task<MaterialType> UpdateAsync(MaterialType materialType)
  {
    materialType.Updated = DateTime.UtcNow;
    context.MaterialTypes.Update(materialType);
    await context.SaveChangesAsync();
    return materialType;
  }

  public async Task<MaterialType?> GetByIdAsync(int materialTypeId)
  {
    return await context.MaterialTypes
      .Include(model => model.ProductTypes)
      .Where(model => model.Id == materialTypeId)
      .SingleOrDefaultAsync();
  }

  public async Task<IEnumerable<MaterialType>> GetAllAsync()
  {
    return await context.MaterialTypes
      .Include(model => model.ProductTypes)
      .ToListAsync();
  }

  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.MaterialTypes.AnyAsync(materialType => materialType.Id == id);
  }

  public async Task<bool> ExistsByTitleAsync(string title)
  {
    return await context.MaterialTypes.AnyAsync(materialType => materialType.Title.ToLower() == title.ToLower());
  }

  public async Task DeleteAsync(MaterialType materialType)
  {
    context.MaterialTypes.Remove(materialType);
    await context.SaveChangesAsync();
  }
}