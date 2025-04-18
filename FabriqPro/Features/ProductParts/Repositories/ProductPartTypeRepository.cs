using FabriqPro.Features.ProductParts.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.ProductParts.Repositories;

public class ProductPartTypeRepository(FabriqDbContext context)
{
    public async Task<ProductPartType> AddAsync(ProductPartType productPartType)
    {
        context.ProductPartTypes.Add(productPartType);
        await context.SaveChangesAsync();
        return productPartType;
    }

    public async Task<ProductPartType> UpdateAsync(ProductPartType productPartType)
    {
        productPartType.Updated = DateTime.UtcNow;
        context.ProductPartTypes.Update(productPartType);
        await context.SaveChangesAsync();
        return productPartType;
    }

    public async Task<ProductPartType?> GetByIdAsync(int id)
    {
        return await context.ProductPartTypes
            .Include(ppt => ppt.ProductType)
            .FirstOrDefaultAsync(ppt => ppt.Id == id);
    }

    public async Task<IEnumerable<ProductPartType>> GetAllAsync()
    {
        return await context.ProductPartTypes
            .Include(ppt => ppt.ProductType)
            .ToListAsync();
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await context.ProductPartTypes.AnyAsync(ppt => ppt.Id == id);
    }

    public async Task<bool> ExistsByTitleAndProductTypeAsync(string title, int productTypeId)
    {
        return await context.ProductPartTypes.AnyAsync(ppt =>
            ppt.Title == title && ppt.ProductTypeId == productTypeId);
    }

    public async Task DeleteAsync(ProductPartType productPartType)
    {
        context.ProductPartTypes.Remove(productPartType);
        await context.SaveChangesAsync();
    }
}