namespace CelebrateME.Models;

public class Order
{
    public int Id { get; set; }
    public string CustomerCode { get; set; } = string.Empty; // login code used
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int TotalPoints { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}