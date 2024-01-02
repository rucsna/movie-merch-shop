using MovieMerchShop.Enum;

namespace MovieMerchShop.Model;

public class Poster:MerchItem
{
    public Size Size { get; set; }
    public Material Material { get; set; }

    public Poster()
    {
    }
}