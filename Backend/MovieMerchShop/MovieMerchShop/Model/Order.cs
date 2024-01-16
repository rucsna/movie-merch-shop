namespace MovieMerchShop.Model;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<MerchItem> Items { get; set; } = new();
    public DateTime OrderTime { get; set; }
    public decimal OrderSum { get; set; }
}