namespace FoodRegistrationTool.Models;

public class Producer
{
    public int ProducerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    // Navigation property
    public virtual List<Product>? Products { get; set; }
}