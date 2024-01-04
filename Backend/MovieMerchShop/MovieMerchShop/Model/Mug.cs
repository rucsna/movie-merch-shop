using MovieMerchShop.Enum;

namespace MovieMerchShop.Model;

public class Mug:MerchItem
{
    public Color Color { get; set; }

    public Mug()
    {
        Type = ItemType.Mug;
    }
}
