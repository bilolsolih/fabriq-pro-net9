namespace FabriqPro.Features.Products.Models;

public class Party
{
  public int Id { get; set; }
  public required string Title { get; set; }

  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}