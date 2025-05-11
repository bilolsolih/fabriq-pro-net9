using System.Data.Entity;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;

namespace FabriqPro.Features.Products.Repositories;

public class MaterialRepository(FabriqDbContext context)
{
  public async Task<Material> AddAsync(Material material)
  {
    context.MaterialTypes.Add(material);
    await context.SaveChangesAsync();
    return material;
  }

  public async Task<Material> UpdateAsync(Material material)
  {
    material.Updated = DateTime.UtcNow;
    context.MaterialTypes.Update(material);
    await context.SaveChangesAsync();
    return material;
  }

  public async Task<Material?> GetByIdAsync(int materialId)
  {
    return await context.MaterialTypes
      // .Include(model => model.ProductType)
      .Where(model => model.Id == materialId)
      .SingleOrDefaultAsync();
  }
  
  public async Task<IEnumerable<Material>> GetAllAsync()
  {
    return await context.MaterialTypes
      // .Include(model => model.ProductType)
      .ToListAsync();
  }
  
  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.MaterialTypes.AnyAsync(material => material.Id == id);
  }
  

  public async Task DeleteAsync(Material material)
  {
    context.MaterialTypes.Remove(material);
    await context.SaveChangesAsync();
  }
}