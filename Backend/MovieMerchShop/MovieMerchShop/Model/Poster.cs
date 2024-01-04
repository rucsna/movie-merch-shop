using MovieMerchShop.Enum;

namespace MovieMerchShop.Model;

public class Poster:MerchItem
{
    public PosterSize Size { get; set; }
    public Material Material { get; set; }

    public Poster()
    {
        Type = ItemType.Poster;
    }
}