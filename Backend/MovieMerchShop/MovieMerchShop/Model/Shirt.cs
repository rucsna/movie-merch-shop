using MovieMerchShop.Enum;

namespace MovieMerchShop.Model;

public class Shirt:MerchItem
{
    public TShirtSize Size { get; set; }
    public Color Color { get; set; }
    

    public Shirt()
    {
        Type = ItemType.Shirt;
    }
}
