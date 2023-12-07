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
    
  
    
    public async Task<Movie> GetMovieByTitle(string title)
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

            return movie;
        }
        else
        {
            _logger.LogError($"Error retrieving movie with title '{title}'. Status code: {response.StatusCode}");
            Console.WriteLine(url);
            return null;
        }  
    }
}

