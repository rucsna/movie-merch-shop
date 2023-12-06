using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieMerchShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _omdbApiKey;

        private static readonly Dictionary<string, List<string>> UserFavoriteMovies =
            new Dictionary<string, List<string>>();


        public MovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _omdbApiKey = "YourOMDBApiKey";
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovieAsync([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("Title is a required parameter.");
            }

            string apiUrl = $"http://www.omdbapi.com/?t={title}";

            using (var httpClient = _httpClientFactory.CreateClient())
            {

                httpClient.DefaultRequestHeaders.Add("apikey", _omdbApiKey);

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(content);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Error: {response.ReasonPhrase}");
                }
            }
        }

        [HttpPost("add-to-favorites")]
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

        [HttpDelete("remove-from-favorites")]
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

        [HttpGet("get-favorite-movies")]
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
    }
}