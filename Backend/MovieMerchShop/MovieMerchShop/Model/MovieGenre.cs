namespace MovieMerchShop.Model;

public class MovieGenre
{
    public Guid Id { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    
    
}