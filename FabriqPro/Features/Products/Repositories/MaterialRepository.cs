using System.Data.Entity;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;

namespace FabriqPro.Features.Products.Repositories;

public class MaterialRepository(FabriqDbContext context)
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

  public async Task<MaterialType?> GetByIdAsync(int materialId)
  {
    return await context.MaterialTypes
      // .Include(model => model.ProductType)
      .Where(model => model.Id == materialId)
      .SingleOrDefaultAsync();
  }
  
  public async Task<IEnumerable<MaterialType>> GetAllAsync()
  {
    return await context.MaterialTypes
      // .Include(model => model.ProductType)
      .ToListAsync();
  }
  
  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.MaterialTypes.AnyAsync(material => material.Id == id);
  }
  

  public async Task DeleteAsync(MaterialType materialType)
  {
    context.MaterialTypes.Remove(materialType);
    await context.SaveChangesAsync();
  }
}