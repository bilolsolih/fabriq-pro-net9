namespace FabriqPro.Core;

public class BaseModel
{
  public int Id { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}

public record BaseModelRecord
{
  public int Id { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}