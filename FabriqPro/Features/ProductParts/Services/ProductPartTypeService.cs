using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.ProductParts.Repositories;

namespace FabriqPro.Features.ProductParts.Services;

public class ProductPartTypeService(ProductPartTypeRepository repository, IMapper mapper)
{
    public async Task<ProductPartTypeDetailDto> GetProductPartTypeByIdAsync(int id)
    {
        var productPartType = await repository.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPartType, nameof(ProductPartType));

        return mapper.Map<ProductPartTypeDetailDto>(productPartType);
    }

    public async Task<IEnumerable<ProductPartTypeListDto>> GetAllProductPartTypesAsync()
    {
        var productPartTypes = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<ProductPartTypeListDto>>(productPartTypes);
    }

    public async Task<ProductPartType> CreateProductPartTypeAsync(ProductPartTypeCreateDto payload)
    {
        var alreadyExists = await repository.ExistsByTitleAndProductTypeAsync(payload.Title, payload.ProductTypeId);
        AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

        return await repository.AddAsync(mapper.Map<ProductPartType>(payload));
    }

    public async Task<ProductPartType> UpdateProductPartTypeAsync(int id, ProductPartTypeUpdateDto payload)
    {
        var productPartType = await repository.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPartType, nameof(ProductPartType));

        mapper.Map(payload, productPartType);
        return await repository.UpdateAsync(productPartType);
    }

    public async Task DeleteProductPartTypeAsync(int id)
    {
        var productPartType = await repository.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPartType, nameof(ProductPartType));

        await repository.DeleteAsync(productPartType);
    }
}