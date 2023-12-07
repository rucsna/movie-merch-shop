using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public interface IOmdbApiProvider
{
    Task<Movie> GetMovieByTitle(string title);
}