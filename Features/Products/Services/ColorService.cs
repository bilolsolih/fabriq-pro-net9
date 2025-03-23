using AutoMapper;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Repositories;

namespace FabriqPro.Features.Products.Services;

public class ColorService(ColorRepository colorRepo, IMapper mapper)
{
  public async Task<ColorDetailDto> GetColorByIdAsync(int id)
  {
    var color = await colorRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(color, nameof(Color));

    return mapper.Map<ColorDetailDto>(color);
  }

  public async Task<IEnumerable<ColorListDto>> GetAllColorsAsync()
  {
    var colors = await colorRepo.GetAllAsync();
    return mapper.Map<IEnumerable<ColorListDto>>(colors);
  }

  public async Task<Color> CreateColorAsync(ColorCreateDto payload)
  {
    var alreadyExists = await colorRepo.ExistsByTitleOrColorCodeAsync(payload.Title, payload.ColorCode);
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    return await colorRepo.AddAsync(mapper.Map<Color>(payload));
  }

  public async Task<Color> UpdateColorAsync(int colorId, ColorUpdateDto payload)
  {
    var color = await colorRepo.GetByIdAsync(colorId);
    DoesNotExistException.ThrowIfNull(color, nameof(Color));

    mapper.Map(payload, color);
    return await colorRepo.UpdateAsync(color);
  }

  public async Task DeleteColorByIdAsync(int id)
  {
    var color = await colorRepo.GetByIdAsync(id);
    DoesNotExistException.ThrowIfNull(color, nameof(Color));

    await colorRepo.DeleteAsync(color);
  }
}