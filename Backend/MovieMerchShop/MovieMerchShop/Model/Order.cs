namespace MovieMerchShop.Model;

public class Order
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public DateTime OrderTime { get; set; }
    public ICollection<MerchItem> OrderItems { get; set; }
}