using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public interface IOmdbApiProvider
{
    Task<ApiResult> GetMovieByTitle(string title);
}