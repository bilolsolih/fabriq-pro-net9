// using FabriqPro.Features.Products.Models;
// using Microsoft.EntityFrameworkCore;
//
// namespace FabriqPro.Features.Products.Repositories;
//
// public class DepartmentRepository(FabriqDbContext context)
// {
//   public async Task<Department> AddAsync(Department department)
//   {
//     context.Departments.Add(department);
//     await context.SaveChangesAsync();
//     return department;
//   }
//
//   public async Task<Department> UpdateAsync(Department department)
//   {
//     department.Updated = DateTime.UtcNow;
//     context.Departments.Update(department);
//     await context.SaveChangesAsync();
//     return department;
//   }
//
//   public async Task<Department?> GetByIdAsync(int departmentId)
//   {
//     return await context.Departments.FindAsync(departmentId);
//   }
//
//   public async Task<IEnumerable<Department>> GetAllAsync()
//   {
//     return await context.Departments.ToListAsync();
//   }
//
//   public async Task<bool> ExistsByIdAsync(int id)
//   {
//     return await context.Departments.AnyAsync(department => department.Id == id);
//   }
//
//   public async Task<bool> ExistsByTitleAsync(string title, bool caseSensitive = false)
//   {
//     if (caseSensitive)
//       return await context.Departments.AnyAsync(department => department.Title == title);
//
//     return await context.Departments.AnyAsync(department => department.Title.ToLower() == title.ToLower());
//   }
//
//   public async Task DeleteAsync(Department department)
//   {
//     context.Departments.Remove(department);
//     await context.SaveChangesAsync();
//   }
// }