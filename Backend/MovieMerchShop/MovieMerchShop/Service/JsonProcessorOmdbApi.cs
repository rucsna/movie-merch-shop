using System.Text.Json;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public class JsonProcessorOmdbApi:IJsonProcessorOmdbApi
{
    private readonly IOmdbApiProvider _omdbApiProvider;
    private readonly AppDbContext _dbContext;
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
                var genre = _dbContext.Genres.FirstOrDefault(g => g.GenreName == genreName) ?? new Genre { GenreName = genreName };
                var movieGenre = new MovieGenre { Movie = movie, Genre = genre };

                _dbContext.MovieGenres.Add(movieGenre);
            }
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing movie: {ex.Message}");
            }
        }
      
        _logger.LogInformation("Movies saved to the database successfully.");
    }
}