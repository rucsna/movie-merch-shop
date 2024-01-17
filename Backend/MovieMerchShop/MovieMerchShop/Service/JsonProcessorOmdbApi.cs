using System.Text.Json;
using MovieMerchShop.Data;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public class JsonProcessorOmdbApi:IJsonProcessorOmdbApi
{
    private readonly AppDbContext _dbContext;
    private readonly IOmdbApiProvider _omdbApiProvider;
    private ILogger<JsonProcessorOmdbApi> _logger;

    public JsonProcessorOmdbApi(IOmdbApiProvider omdbApiProvider, ILogger<JsonProcessorOmdbApi> logger,AppDbContext dbContext)
    {
        _omdbApiProvider = omdbApiProvider;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task GetMovies()
    {
        string jsonText = File.ReadAllText("Data/Movies.json");
        JsonDocument jsonDocument = JsonDocument.Parse(jsonText);
        
        foreach (var moviesDataElement in jsonDocument.RootElement.EnumerateArray())
        {
            try
            {
                string title = moviesDataElement.GetString();
                Console.WriteLine(title);
                var result = await _omdbApiProvider.GetMovieByTitle(title);
                Movie movie = result.Movie;
            
                if (movie != null)
                {
                    _dbContext.Movies.Add(movie);
                }
                foreach (var genreName in result.Genres)
                {
                    var genreInDb = _dbContext.Genres.FirstOrDefault(g => g.GenreName == genreName);
                    if (genreInDb == null)
                    {
                        var genreToAdd = new Genre { Id = Guid.NewGuid(), GenreName = genreName };
                        _dbContext.Genres.Add(genreToAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing movie: {ex.Message}");
            }
        }
        await _dbContext.SaveChangesAsync();
      
        _logger.LogInformation("Movies saved to the database successfully.");
    }
}