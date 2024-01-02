namespace MovieMerchShop.Model;

public class MerchItem
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public Guid MovieId { get; set; }
    public uint Quantity { get; set; }

    public MerchItem()
    {
    }
}