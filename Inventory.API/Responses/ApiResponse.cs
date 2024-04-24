namespace Inventory.API.Responses;

public class ApiResponse
{
    public string Status { get; set; }
    public string StatusText { get; set; }
    public object Data { get; set; }
}