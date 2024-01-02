using MovieMerchShop.Enum;

namespace MovieMerchShop.Model;

public class Shirt:MerchItem
{
    public Size Size { get; set; }
    public Color Color { get; set; }

    public Shirt()
    {
    }
}
