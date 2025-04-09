using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Repositories;

namespace FabriqPro.Features.Products.Services;

public class MaterialTypeService(
  MaterialTypeRepository materialTypeRepo,
  ProductTypeRepository productTypeRepo,
  IMapper mapper)
{
  public async Task<MaterialTypeDetailDto> GetMaterialTypeByIdAsync(int id)
  {
    var materialType = await materialTypeRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(materialType, nameof(MaterialType));

    return mapper.Map<MaterialTypeDetailDto>(materialType);
  }

  public async Task<IEnumerable<MaterialTypeListDto>> GetAllMaterialTypesAsync()
  {
    var materialTypes = await materialTypeRepo.GetAllAsync();
    return mapper.Map<List<MaterialTypeListDto>>(materialTypes);
  }

  public async Task<MaterialType> CreateMaterialTypeAsync(MaterialTypeCreateDto payload)
  {
    var alreadyExists = await materialTypeRepo.ExistsByTitleAsync(payload.Title);
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    if (!payload.ProductTypeIds.Any())
    {
      throw new ArgumentException(nameof(payload.ProductTypeIds));
    }

    var newMaterialType = mapper.Map<MaterialType>(payload);

    foreach (int productTypeId in payload.ProductTypeIds)
    {
      var productType = await productTypeRepo.GetByIdAsync(productTypeId);
      DoesNotExistException.ThrowIfNull(productType, nameof(ProductType));

      newMaterialType.ProductTypes.Add(productType);
    }

    return await materialTypeRepo.AddAsync(newMaterialType);
  }

  public async Task DeleteMaterialTypeByIdAsync(int id)
  {
    var materialType = await materialTypeRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(materialType, nameof(MaterialType));

    await materialTypeRepo.DeleteAsync(materialType);
  }
}