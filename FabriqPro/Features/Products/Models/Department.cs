// using FabriqPro.Features.Authentication.Models;
//
// namespace FabriqPro.Features.Products.Models;
//
// public class Department
// {
//   public int Id { get; set; }
//   public required string Title { get; set; }
//
//   public ICollection<Product> Products { get; set; } = new List<Product>();
//   public ICollection<User> Users { get; set; } = new List<User>();
//   
//   public DateTime Created { get; set; }
//   public DateTime Updated { get; set; }
// }

namespace FabriqPro.Features.Products.Models;

public enum Department
{
  Storage,
  Cutting,
  Sewing,
  Cleaning,
  Pressing,
  Packaging
}