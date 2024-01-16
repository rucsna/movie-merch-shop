namespace MovieMerchShop.Model;

public class MerchItem
{
    public Guid Id { get; set; }
    public ItemType Type { get; set; }
    public decimal Price { get; set; }
    public Guid MovieId { get; set; }
    public int Quantity { get; set; }
    public List<Order> Orders { get; set; } = new();
}