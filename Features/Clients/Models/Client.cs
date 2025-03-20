namespace FabriqPro.Features.Clients.Models;

public class Client
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}