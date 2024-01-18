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
        Assert.NotNull(authResponse!.Token);
        Assert.Equal("user2@gmail.com", authResponse.Email);
        Assert.Equal("user2", authResponse.UserName);
    }

    [Fact]
    public async Task Test_FailedLogin_WithInvalidUserData()
    {
        // Invalid Login Attempt
        var loginRequest = new AuthRequest("invalidUserName@email.com", "invalidPassword");
        var loginResponse = await _client.PostAsync("api/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json"));
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
    }

    [Fact]
    public async Task Test_Registration_ReturnsOk_WhenSuccessfulRegistration()
    {
        // Registration Attempt
        var registrationRequest = new RegistrationRequest("test3@test.com", "tester3", "testPassword",
            new DateTime(2000, 12, 12), "tester3Address");

        var registrationResponse = await _client.PostAsync("api/Auth/Register",
            new StringContent(JsonConvert.SerializeObject(registrationRequest), Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, registrationResponse.StatusCode);
        
        // Delete test user from database
        var deleteResponse = await _client.DeleteAsync("api/Auth/DeleteTestUser");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Test_RegistrationFails_WhenInputIsInValid()
    {
        
    }


    void IDisposable.Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}