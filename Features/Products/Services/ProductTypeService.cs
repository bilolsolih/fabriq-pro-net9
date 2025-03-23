using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Repositories;

namespace FabriqPro.Features.Products.Services;

public class ProductTypeService(ProductTypeRepository productTypeRepo, IMapper mapper)
{
  public async Task<ProductTypeDetailDto> GetProductTypeByIdAsync(int id)
  {
    var productType = await productTypeRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(productType, nameof(ProductType));

    return mapper.Map<ProductTypeDetailDto>(productType);
  }

  public async Task<IEnumerable<ProductTypeListDto>> GetAllProductTypesAsync()
  {
    var productTypes = await productTypeRepo.GetAllAsync();
    return mapper.Map<IEnumerable<ProductTypeListDto>>(productTypes);
  }

  public async Task<ProductType> CreateProductTypeAsync(ProductTypeCreateDto payload)
  {
    var alreadyExists = await productTypeRepo.ExistsByTitleAsync(payload.Title);
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    return await productTypeRepo.AddAsync(mapper.Map<ProductType>(payload));
  }

  public async Task<ProductType> UpdateProductTypeAsync(int productTypeId, ProductTypeUpdateDto payload)
  {
    var productType = await productTypeRepo.GetByIdAsync(productTypeId);
    DoesNotExistException.ThrowIfNull(productType, nameof(ProductType));

    mapper.Map(payload, productType);
    return await productTypeRepo.UpdateAsync(productType);
  }

  public async Task DeleteProductTypeByIdAsync(int id)
  {
    var productType = await productTypeRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(productType, nameof(ProductType));

    await productTypeRepo.DeleteAsync(productType);
  }
}