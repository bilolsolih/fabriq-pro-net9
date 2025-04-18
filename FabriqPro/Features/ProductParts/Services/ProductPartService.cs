using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.ProductParts.Repositories;

namespace FabriqPro.Features.ProductParts.Services;

public class ProductPartService(ProductPartRepository productPartRepo, IMapper mapper)
{
    public async Task<ProductPartDetailDto> GetProductPartByIdAsync(int id)
    {
        var productPart = await productPartRepo.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPart, nameof(ProductPart));

        return mapper.Map<ProductPartDetailDto>(productPart);
    }

    public async Task<IEnumerable<ProductPartListDto>> GetAllProductPartsAsync()
    {
        var productParts = await productPartRepo.GetAllAsync();
        return mapper.Map<IEnumerable<ProductPartListDto>>(productParts);
    }

    public async Task<ProductPart> CreateProductPartAsync(ProductPartCreateDto payload)
    {
        var alreadyExists = await productPartRepo.ExistsByIdsAsync(payload.ProductTypeId, payload.ProductPartTypeId, payload.ProductModelId);
        AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

        return await productPartRepo.AddAsync(mapper.Map<ProductPart>(payload));
    }

    public async Task<ProductPart> UpdateProductPartAsync(int productPartId, ProductPartUpdateDto payload)
    {
        var productPart = await productPartRepo.GetByIdAsync(productPartId);
        DoesNotExistException.ThrowIfNull(productPart, nameof(ProductPart));

        mapper.Map(payload, productPart);
        return await productPartRepo.UpdateAsync(productPart);
    }

    public async Task DeleteProductPartByIdAsync(int id)
    {
        var productPart = await productPartRepo.GetByIdAsync(id);
        DoesNotExistException.ThrowIfNull(productPart, nameof(ProductPart));

        await productPartRepo.DeleteAsync(productPart);
    }
}