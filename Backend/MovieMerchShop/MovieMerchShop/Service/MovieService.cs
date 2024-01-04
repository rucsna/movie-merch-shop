using MovieMerchShop.Data;

namespace MovieMerchShop.Service;

public class MovieService
{
    private readonly AppDbContext _context;

    public MovieService(AppDbContext context)
    {
        _context = context;
    }

    public List<Guid> GetMovieIds()
    {
        return _context.Movies.Select(movie => movie.Id).ToList();
    }
}