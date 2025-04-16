using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.ProductParts.Repositories;

namespace FabriqPro.Features.ProductParts.Services;

public class ProductPartToDepartmentService(ProductPartToDepartmentRepository repository, IMapper mapper)
{
    public async Task<ProductPartToDepartmentDetailDto> GetProductPartToDepartmentByIdAsync(int id)
    {
        var productPartToDepartment = await repository.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPartToDepartment, nameof(ProductPartToDepartment));

        return mapper.Map<ProductPartToDepartmentDetailDto>(productPartToDepartment);
    }

    public async Task<IEnumerable<ProductPartToDepartmentListDto>> GetAllProductPartToDepartmentsAsync()
    {
        var productPartToDepartments = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<ProductPartToDepartmentListDto>>(productPartToDepartments);
    }

    public async Task<ProductPartToDepartment> CreateProductPartToDepartmentAsync(ProductPartToDepartmentCreateDto payload)
    {
        var alreadyExists = await repository.ExistsByProductPartAndDepartmentAsync(payload.ProductPartId, payload.DepartmentId);
        AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

        return await repository.AddAsync(mapper.Map<ProductPartToDepartment>(payload));
    }

    public async Task<ProductPartToDepartment> UpdateProductPartToDepartmentAsync(int id, ProductPartToDepartmentUpdateDto payload)
    {
        var productPartToDepartment = await repository.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPartToDepartment, nameof(ProductPartToDepartment));

        mapper.Map(payload, productPartToDepartment);
        return await repository.UpdateAsync(productPartToDepartment);
    }

    public async Task DeleteProductPartToDepartmentAsync(int id)
    {
        var productPartToDepartment = await repository.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPartToDepartment, nameof(ProductPartToDepartment));

        await repository.DeleteAsync(productPartToDepartment);
    }
}