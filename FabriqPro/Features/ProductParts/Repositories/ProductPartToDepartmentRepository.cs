using FabriqPro.Features.ProductParts.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.ProductParts.Repositories;

public class ProductPartToDepartmentRepository(FabriqDbContext context)
{
    public async Task<ProductPartToDepartment> AddAsync(ProductPartToDepartment productPartToDepartment)
    {
        context.ProductPartToDepartments.Add(productPartToDepartment);
        await context.SaveChangesAsync();
        return productPartToDepartment;
    }

    public async Task<ProductPartToDepartment> UpdateAsync(ProductPartToDepartment productPartToDepartment)
    {
        productPartToDepartment.Updated = DateTime.UtcNow;
        context.ProductPartToDepartments.Update(productPartToDepartment);
        await context.SaveChangesAsync();
        return productPartToDepartment;
    }

    public async Task<ProductPartToDepartment?> GetByIdAsync(int id)
    {
        return await context.ProductPartToDepartments
            .Include(ppd => ppd.ProductPart)
            .Include(ppd => ppd.Department)
            .FirstOrDefaultAsync(ppd => ppd.Id == id);
    }

    public async Task<IEnumerable<ProductPartToDepartment>> GetAllAsync()
    {
        return await context.ProductPartToDepartments
            .Include(ppd => ppd.ProductPart)
            .Include(ppd => ppd.Department)
            .ToListAsync();
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await context.ProductPartToDepartments.AnyAsync(ppd => ppd.Id == id);
    }

    public async Task<bool> ExistsByProductPartAndDepartmentAsync(int productPartId, int department)
    {
        return await context.ProductPartToDepartments.AnyAsync(ppd =>
            ppd.ProductPartId == productPartId);
        // ppd.DepartmentId == department);
    }

    public async Task DeleteAsync(ProductPartToDepartment productPartToDepartment)
    {
        context.ProductPartToDepartments.Remove(productPartToDepartment);
        await context.SaveChangesAsync();
    }
}