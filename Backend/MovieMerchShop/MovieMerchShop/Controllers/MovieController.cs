using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using MovieMerchShop.Data;
using MovieMerchShop.Model;
using MovieMerchShop.Service;

namespace MovieMerchShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
       // private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _omdbApiKey;
        private readonly IOmdbApiProvider _omdbApiProvider;
        private readonly IJsonProcessorOmdbApi _jsonProcessor;
        private ILogger<MovieController> _logger;
        private readonly AppDbContext _context;
        private static readonly Dictionary<string, List<string>> UserFavoriteMovies =
            new Dictionary<string, List<string>>();


        public MovieController(AppDbContext context,IOmdbApiProvider omdbApiProvider,IJsonProcessorOmdbApi jsonProcessorOmdbApi, ILogger<MovieController> logger)

        {

            //_httpClientFactory = httpClientFactory;
            _omdbApiKey = "YourOMDBApiKey";
            _omdbApiProvider = omdbApiProvider;
            _logger = logger;
            _jsonProcessor = jsonProcessorOmdbApi;
            _context = context;
        }

        [HttpGet("getMoviesToLandingPage")]
        public IActionResult GetMoviesToLandingPage()
        {
            try
            {
                var items = _context.Movies.ToList();

                // Shuffle the list so every time 20 random movie would be chosen
                var random = new Random();
                var shuffledList = items.OrderBy(x => random.Next()).ToList();
                
                var shortList = shuffledList.Take(20).ToList();
                return Ok(shortList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Error getting movies");
            }
        }
       
        
        [HttpGet("filldata")]
        public async Task<IActionResult> MoviesDataFromApi()
        {
            try
            {
                await _jsonProcessor.GetMovies();
                return Ok("Movies saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving movies to the database: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        [HttpPost("addtofavorites")]
        public IActionResult AddToFavoriteMovies([FromQuery] string username, [FromQuery] string movieTitle)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(movieTitle))
            {
                return BadRequest("Username and movieTitle are required parameters.");
            }

            if (!UserFavoriteMovies.ContainsKey(username))
            {
                UserFavoriteMovies[username] = new List<string>();
            }

            if (!UserFavoriteMovies[username].Contains(movieTitle))
            {
                UserFavoriteMovies[username].Add(movieTitle);
                return Ok($"Added '{movieTitle}' to {username}'s favorite movies.");
            }
            else
            {
                return BadRequest($"'{movieTitle}' is already in {username}'s favorite movies.");
            }
        }

        [HttpDelete("removefromfavorites")]
        public IActionResult RemoveFromFavoriteMovies([FromQuery] string username, [FromQuery] string movieTitle)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(movieTitle))
            {
                return BadRequest("Username and movieTitle are required parameters.");
            }

            if (UserFavoriteMovies.ContainsKey(username) && UserFavoriteMovies[username].Contains(movieTitle))
            {
                UserFavoriteMovies[username].Remove(movieTitle);
                return Ok($"Removed '{movieTitle}' from {username}'s favorite movies.");
            }
            else
            {
                return NotFound($"'{movieTitle}' not found in {username}'s favorite movies.");
            }
        }

        [HttpGet("getfavoritemovies")]
        public IActionResult GetFavoriteMovies([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is a required parameter.");
            }

            if (UserFavoriteMovies.ContainsKey(username))
            {
                return Ok(UserFavoriteMovies[username]);
            }
            else
            {
                return NotFound($"{username} does not have any favorite movies.");
            }
        }
        
        [HttpGet("GetMoviesByTitle/{title}")]
        public IActionResult GetMoviesByTitle(string title)
        {
            var movies = _context.Movies.Where(movie => movie.Title.Contains(title)).ToList();
            if (movies ==null || movies.Count ==0)
            {
                return NotFound();
            }

            return Ok(movies);
        }
    }
}