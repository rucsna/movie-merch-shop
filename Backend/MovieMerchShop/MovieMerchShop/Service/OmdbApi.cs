using System.Text.Json;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public class OmdbApi : IOmdbApiProvider
{
    private readonly ILogger<OmdbApi> _logger;

    public OmdbApi(ILogger<OmdbApi> logger)
    {
        _logger = logger;
    }



    public async Task<ApiResult> GetMovieByTitle(string title)
    {
        var ApiKey = "8136c53";
        var url = $"http://www.omdbapi.com/?apikey={ApiKey}&t={title}";
        var client = new HttpClient();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            
            var movie = JsonSerializer.Deserialize<Movie>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            GenreList genreList = JsonSerializer.Deserialize < GenreList > (json);
            
            if (movie != null)
            {
                //var genres = movie.Genres?.Select(g => g.Genre.GenreName).ToList() ?? new List<string>();
               // Console.WriteLine(genres.Count);
                var apiResult = new ApiResult
                {
                    Movie = movie,
                    Genres = genreList.Genre.Split(',').Select(s=>s.Trim()).ToList()
                };

                return apiResult;
            }
            else
            {
                _logger.LogError($"Error retrieving movie with title '{title}'. Movie is null.");
                return null;
            }
        }
        else
        {
            _logger.LogError($"Error retrieving movie with title '{title}'. Status code: {response.StatusCode}");
            Console.WriteLine(url);
            return null;
        }

    }
}




