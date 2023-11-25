namespace MovieMerchShop.Model;

public abstract class MerchItem
{
    public Guid Id { get; }
    public decimal Price { get; set; }
    public Guid MovieId { get; }
    public uint Quantity { get; set; }
}