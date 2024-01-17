namespace MovieMerchShop.Model;

public class Genre
{
    public Guid Id { get; set; }
    public string GenreName { get; set; }
    public List<Movie> Movies { get; set; } = new();
};