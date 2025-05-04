// using AutoMapper;
// using FabriqPro.Core.Exceptions;
// using FabriqPro.Features.Products.DTOs;
// using FabriqPro.Features.Products.Models;
// using FabriqPro.Features.Products.Repositories;
//
// namespace FabriqPro.Features.Products.Services;
//
// public class DepartmentService(DepartmentRepository departmentRepo, IMapper mapper)
// {
//   public async Task<DepartmentDetailDto> GetDepartmentByIdAsync(int id)
//   {
//     var department = await departmentRepo.GetByIdAsync(id);
//     DoesNotExistException.ThrowIfNull(department, nameof(Department));
//
//     return mapper.Map<DepartmentDetailDto>(department);
//   }
//
//   public async Task<IEnumerable<DepartmentListDto>> GetAllDepartmentsAsync()
//   {
//     var departments = await departmentRepo.GetAllAsync();
//     return mapper.Map<IEnumerable<DepartmentListDto>>(departments);
//   }
//
//   public async Task<Department> CreateDepartmentAsync(DepartmentCreateDto payload)
//   {
//     var alreadyExists = await departmentRepo.ExistsByTitleAsync(payload.Title);
//     AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());
//
//     return await departmentRepo.AddAsync(mapper.Map<Department>(payload));
//   }
//
//   public async Task<Department> UpdateDepartmentAsync(int departmentId, DepartmentUpdateDto payload)
//   {
//     var department = await departmentRepo.GetByIdAsync(departmentId);
//     DoesNotExistException.ThrowIfNull(department, nameof(Department));
//
//     mapper.Map(payload, department);
//     return await departmentRepo.UpdateAsync(department);
//   }
//
//   public async Task DeleteDepartmentByIdAsync(int id)
//   {
//     var department = await departmentRepo.GetByIdAsync(id);
//     DoesNotExistException.ThrowIfNull(department, nameof(Department));
//
//     await departmentRepo.DeleteAsync(department);
//   }
// }