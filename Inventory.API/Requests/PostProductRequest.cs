namespace Inventory.API.Requests;

public class PostProductRequest
{
    required public string Name { get; set; }
    required public double Price { get; set; }
}