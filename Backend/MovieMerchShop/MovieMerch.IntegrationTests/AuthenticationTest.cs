using System.Net;
using System.Text;
using MovieMerchShop.Contracts;
using MovieMerchShop.Service.Authentication;
using Newtonsoft.Json;

namespace MovieMerch.IntegrationTests;

public class AuthenticationTest : IDisposable
{
    private readonly MovieMerchFactory _factory;
    private readonly HttpClient _client;

    public AuthenticationTest()
    {
        _factory = new MovieMerchFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Test_Login_ReturnsOK_WhenUserValid()
    {
        // Login Attempt
        var loginRequest = new AuthRequest("user2@gmail.com", "password");
        var loginResponse = await _client.PostAsync("api/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());

        // Assert Login
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.NotNull(authResponse.Token);
        Assert.Equal("user2@gmail.com", authResponse.Email);
        Assert.Equal("user2", authResponse.UserName);
    }
    
    

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}