using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Repositories;

namespace FabriqPro.Features.Products.Services;

public class ProductModelService(
  ProductModelRepository productModelRepo,
  ColorRepository colorRepo,
  ProductTypeRepository productTypeRepo,
  IMapper mapper
)
{
  public async Task<ProductModelDetailDto> GetProductModelByIdAsync(int id)
  {
    var productModel = await productModelRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(productModel, nameof(ProductModel));

    return mapper.Map<ProductModelDetailDto>(productModel);
  }

  public async Task<IEnumerable<ProductModelListDto>> GetAllProductModelsAsync()
  {
    var productModels = await productModelRepo.GetAllAsync();
    return mapper.Map<IEnumerable<ProductModelListDto>>(productModels);
  }

  public async Task<ProductModel> CreateProductModelAsync(ProductModelCreateDto payload)
  {
    var alreadyExists = await productModelRepo.ExistsByTitleAsync(payload.Title);
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var colorExists = await colorRepo.ExistsByIdAsync(payload.ColorId);
    DoesNotExistException.ThrowIfNot(colorExists, $"Color with Id: {payload.ColorId}");

    var productTypeExists = await productTypeRepo.ExistsByIdAsync(payload.ProductTypeId);
    DoesNotExistException.ThrowIfNot(productTypeExists, $"ProductType with Id: {payload.ProductTypeId}");

    return await productModelRepo.AddAsync(mapper.Map<ProductModel>(payload));
  }

  public async Task<ProductModel> UpdateProductModelAsync(int productModelId, ProductModelUpdateDto payload)
  {
    var productModel = await productModelRepo.GetByIdAsync(productModelId);
    DoesNotExistException.ThrowIfNull(productModel, nameof(ProductModel));

    if (!string.IsNullOrEmpty(payload.Title) && productModel.Title.ToLower() != payload.Title.ToLower())
    {
      var alreadyExists = await productModelRepo.ExistsByTitleAsync(payload.Title);
      AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());
    }

    if (payload.ColorId.HasValue && productModel.ColorId != payload.ColorId)
    {
      var colorExists = await colorRepo.ExistsByIdAsync((int)payload.ColorId);
      DoesNotExistException.ThrowIfNot(colorExists, $"Color with Id: {payload.ColorId}");
    }

    if (payload.ProductTypeId.HasValue && productModel.ProductTypeId != payload.ProductTypeId)
    {
      var productTypeExists = await productTypeRepo.ExistsByIdAsync((int)payload.ProductTypeId);
      DoesNotExistException.ThrowIfNot(productTypeExists, $"ProductType with Id: {payload.ProductTypeId}");
    }

    mapper.Map(payload, productModel);
    return await productModelRepo.UpdateAsync(productModel);
  }

  public async Task DeleteProductModelByIdAsync(int id)
  {
    var productModel = await productModelRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(productModel, nameof(ProductModel));
    // TODO: CRITICAL! Must check if there are any Products related to the model before attempting to delete it.
    // Otherwise, Unhandled exception will be thrown by the Database itself.

    await productModelRepo.DeleteAsync(productModel);
  }
}