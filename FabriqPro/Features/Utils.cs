using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features;

public class Utils
{
  public static string GetTitleForUnit(Unit unit)
  {
    var map = new Dictionary<Unit, string>
    {
      { Unit.Piece, "dona" },
      { Unit.Kg, "kg" },
      { Unit.Meter, "metr" },
    };

    return map[unit];
  }

  public static string GetTitleForRole(UserRoles role)
  {
    var map = new Dictionary<UserRoles, string>
    {
      { UserRoles.SuperAdmin, "Super admin" },
      { UserRoles.Supplier, "Yetkazuvchi" },
      { UserRoles.StorageManager, "Omborchi" },
      { UserRoles.CuttingMaster, "Kesuv master" },
      { UserRoles.SewingMaster, "Tikuv master" },
      { UserRoles.Cutter, "Kesuvchi" },
      { UserRoles.Sewer, "Tikuvchi" },
      { UserRoles.Packager, "Qadoqlovchi" },
      { UserRoles.Presser, "Dazmolchi" },
    };

    if (map.ContainsKey(role))
    {
      return map[role];
    }


    return "Noma'lum";
  }
}