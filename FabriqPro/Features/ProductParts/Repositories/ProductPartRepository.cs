using FabriqPro.Features.ProductParts.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.ProductParts.Repositories;

public class ProductPartRepository(FabriqDbContext context)
{
    public async Task<ProductPart> AddAsync(ProductPart productPart)
    {
        context.ProductParts.Add(productPart);
        await context.SaveChangesAsync();
        return productPart;
    }

    public async Task<ProductPart> UpdateAsync(ProductPart productPart)
    {
        productPart.Updated = DateTime.UtcNow;
        context.ProductParts.Update(productPart);
        await context.SaveChangesAsync();
        return productPart;
    }

    public async Task<ProductPart?> GetByIdAsync(int productPartId)
    {
        return await context.ProductParts
            .Include(pp => pp.ProductType)
            .Include(pp => pp.ProductPartType)
            .Include(pp => pp.ProductModel)
            .FirstOrDefaultAsync(pp => pp.Id == productPartId);
    }

    public async Task<IEnumerable<ProductPart>> GetAllAsync()
    {
        return await context.ProductParts
            .Include(pp => pp.ProductType)
            .Include(pp => pp.ProductPartType)
            .Include(pp => pp.ProductModel)
            .ToListAsync();
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await context.ProductParts.AnyAsync(pp => pp.Id == id);
    }

    public async Task<bool> ExistsByIdsAsync(int productTypeId, int productPartTypeId, int productModelId)
    {
        return await context.ProductParts.AnyAsync(pp =>
            pp.ProductTypeId == productTypeId &&
            pp.ProductPartTypeId == productPartTypeId &&
            pp.ProductModelId == productModelId);
    }

    public async Task DeleteAsync(ProductPart productPart)
    {
        context.ProductParts.Remove(productPart);
        await context.SaveChangesAsync();
    }
}