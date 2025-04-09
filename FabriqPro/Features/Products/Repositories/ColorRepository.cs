using FabriqPro.Features.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Repositories;

public class ColorRepository(FabriqDbContext context)
{
  public async Task<Color> AddAsync(Color color)
  {
    context.Colors.Add(color);
    await context.SaveChangesAsync();
    return color;
  }

  public async Task<Color> UpdateAsync(Color color)
  {
    color.Updated = DateTime.UtcNow;
    context.Colors.Update(color);
    await context.SaveChangesAsync();
    return color;
  }

  public async Task<Color?> GetByIdAsync(int colorId)
  {
    return await context.Colors.FindAsync(colorId);
  }

  public async Task<IEnumerable<Color>> GetAllAsync()
  {
    return await context.Colors.ToListAsync();
  }

  public async Task<bool> ExistsByIdAsync(int id)
  {
    return await context.Colors.AnyAsync(color => color.Id == id);
  }

  public async Task<bool> ExistsByTitleOrColorCodeAsync(string title, string colorCode)
  {
    return await context.Colors.AnyAsync(color => color.Title == title || color.ColorCode == colorCode);
  }

  public async Task DeleteAsync(Color color)
  {
    context.Colors.Remove(color);
    await context.SaveChangesAsync();
  }
}