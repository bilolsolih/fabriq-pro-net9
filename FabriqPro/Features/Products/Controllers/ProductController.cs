using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/products"), Authorize]
public class ProductController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
  // [HttpPost("create-product-type"), Authorize(Policy = "SuperAdmin")]
  // public async Task<ActionResult<ProductTypeCreateDto>> CreateProductType(ProductTypeCreateDto payload)
  // {
  //   var userId = int.Parse(User.FindFirstValue("id")!);
  //   var user = await context.Users.FindAsync(userId);
  //   DoesNotExistException.ThrowIfNull(user, "Qaytadan login qiling va yana urinib ko'ring.");
  //
  //   var alreadyExists = await context.ProductTypes.AnyAsync(p => p.Title.ToLower() == payload.Title.ToLower());
  //   AlreadyExistsException.ThrowIf(alreadyExists, "Bunaqa nomli Maxsulot turi allaqachon mavjud.");
  //
  //   var newProductType = new ProductType
  //   {
  //     Title = payload.Title
  //   };
  //
  //   context.ProductTypes.Add(newProductType);
  //   await context.SaveChangesAsync();
  //   return Ok(payload);
  // }

  [HttpGet("list-all-products"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<IEnumerable<ProductListDto>>> ListAllProductsInStorage()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qiling va yana urinib ko'ring.");

    var products = await context.Products.ProjectTo<ProductListDto>(mapper.ConfigurationProvider).ToListAsync();
    return Ok(products);
  }

  [HttpPost("add-product-to-master"), Authorize(Policy = "SewingMasterOrSuperAdmin")]
  public async Task<ActionResult<AddProductToMasterDto>> AddProductToMaster(AddProductToMasterDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qiling va yana urinib ko'ring.");

    var productTypeExists = await context.ProductTypes.AnyAsync(p => p.Id == payload.ProductTypeId);
    DoesNotExistException.ThrowIfNot(productTypeExists, "Bunday Maxsulot turi mavjud emas.");

    var productModelExists = await context.ProductModels
      .AnyAsync(p => p.Id == payload.ProductModelId && p.ProductTypeId == payload.ProductTypeId);

    DoesNotExistException.ThrowIfNot(productModelExists, "Maxsulotning bunday modeli mavjud emas.");

    var addedProduct = new Product
    {
      Department = Department.Sewing,
      MasterId = user.Id,
      ProductTypeId = payload.ProductTypeId,
      ProductModelId = payload.ProductModelId,
      Quantity = payload.Quantity,
      Status = ItemStatus.AddedByMaster,
    };

    context.Products.Add(addedProduct);
    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpPatch("update-added-product/{id:int}"), Authorize(Policy = "SewingMasterOrSuperAdmin")]
  public async Task<ActionResult<UpdateAddedProductDto>> UpdateAddedProduct(int id, UpdateAddedProductDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qiling va yana urinib ko'ring.");

    var product = await context.Products.FindAsync(id);
    DoesNotExistException.ThrowIfNull(product, "O'zgaritilmoqchi bo'lyapgan maxsulot mavjud emas.");

    if (payload is { ProductTypeId: not null })
    {
      var productTypeExists = await context.ProductTypes.AnyAsync(p => p.Id == payload.ProductTypeId);
      DoesNotExistException.ThrowIfNot(productTypeExists, "Bunday Maxsulot turi mavjud emas.");
      product.ProductTypeId = (int)payload.ProductTypeId;
    }

    if (payload is { ProductModelId: not null })
    {
      var productModelExists = await context.ProductModels
        .AnyAsync(p => p.Id == payload.ProductModelId && p.ProductTypeId == payload.ProductTypeId);
      DoesNotExistException.ThrowIfNot(productModelExists, "Maxsulotning bunday modeli mavjud emas.");
      product.ProductModelId = (int)payload.ProductModelId;
    }

    if (payload is { Quantity: not null })
    {
      product.Quantity = (int)payload.Quantity;
    }

    context.Products.Update(product);
    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpGet("list-products-added-by-me"), Authorize(Policy = "SewingMasterOrSuperAdmin")]
  public async Task<ActionResult<ProductsAddedByMeListDto>> ListProductsAddedByMe()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qiling va yana urinib ko'ring.");

    var products = await context.Products
      .Where(p => p.MasterId == user.Id)
      .ProjectTo<ProductsAddedByMeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(products);
  }

  [HttpPost("give-products-to-master"), Authorize(Policy = "SewingMasterOrSuperAdmin")]
  public async Task<ActionResult<GiveProductToMaster>> GiveProductToMaster(GiveProductToMaster payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var fromUser = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(fromUser, "Qaytadan login qilib yana urinib ko'ring.");

    var toUser = await context.Users.FindAsync(payload.MasterId);
    DoesNotExistException.ThrowIfNull(toUser, "Maxsulot mavjud bo'lmagan Masterga o'tkazilmoqda.");

    if (toUser.Role != UserRoles.PackagingMaster)
    {
      return BadRequest("Faqat Qadoqlash Masteriga o'tkazib berish mumkin.");
    }

    var source = await context.Products.FindAsync(payload.ProductId);
    DoesNotExistException.ThrowIfNull(source, "Bunday maxsulot mavjud emas.");

    if (source.MasterId != fromUser.Id)
    {
      return Forbid("Sizning nomingizda bo'lmagan maxsulotni o'tkazib bera olmaysiz.");
    }

    if (payload.Quantity > source.Quantity)
    {
      return BadRequest("O'tkazilayapgan maxsulot miqdori qolgan maxsulot miqdoridan ko'p.");
    }

    var product = source with
    {
      OriginId = source.Id,
      Department = Department.Packaging,
      FromUserId = fromUser.Id,
      ToUserId = toUser.Id,
      Quantity = payload.Quantity,
      Status = ItemStatus.Pending,
    };

    source.Quantity -= payload.Quantity;

    context.Products.Add(product);
    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("list-products-sent-to-me"), Authorize(Policy = "PackagingMasterOrStorageManagerOrSuperAdmin")]
  public async Task<ActionResult<ProductsAddedByMeListDto>> ListProductsSentToMe()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qiling va yana urinib ko'ring.");

    var products = await context.Products
      .Where(p => p.ToUserId == user.Id)
      .ProjectTo<ProductsSentToMeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(products);
  }
}