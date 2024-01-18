using System.Net;
using System.Net.Http.Headers;
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
    public async Task Test_RegistrationFails_WhenInputIsInvalid()
    {
        // Invalid Registration Attempt
        var registrationRequest = new RegistrationRequest("test.test", "example", "password", new DateTime(2000, 12, 12),
            "testerAddress");
        var registrationResponse = await _client.PostAsync("api/Auth/Register", new StringContent(JsonConvert.SerializeObject(registrationRequest), Encoding.UTF8, "application/json"));
        var responseContent = await registrationResponse.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, registrationResponse.StatusCode);
        Assert.Contains("Email 'test.test' is invalid.", responseContent);
    }
    
    [Fact]
    public async Task Test_RegistrationFails_WhenUserNameOrEmail_AlreadyExists()
    {
        // Invalid Registration Attempt
        var registrationRequest = new RegistrationRequest("user2@gmail.com", "tester", "password", new DateTime(2000, 12, 12),
            "testerAddress");
        var registrationResponse = await _client.PostAsync("api/Auth/Register", new StringContent(JsonConvert.SerializeObject(registrationRequest), Encoding.UTF8, "application/json"));
        var responseContent = await registrationResponse.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, registrationResponse.StatusCode);
        Assert.Contains("Email 'user2@gmail.com' is already taken.", responseContent);
        Assert.Contains("Username 'tester' is already taken.", responseContent);
    }

    [Fact]
    public async Task Test_AddOrder_WithAuthorization()
    {
        // Login Attempt to Obtain Token
        var loginRequest = new AuthRequest("user2@gmail.com", "password");
        var loginResponse = await _client.PostAsync("api/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        var userToken = authResponse.Token;
        
        // Assert
        Assert.NotNull(authResponse.Token);
        Assert.Equal("user2@gmail.com", authResponse.Email);
        Assert.Equal("user2", authResponse.UserName);

        // Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var orderRequest = new OrderRequest("user2@gmail.com",
            new List<Guid> { new Guid("af149e06-c691-48bc-6ea4-08dc1731afa1") }, 15);

        var orderResponse = await _client.PostAsync("/api/Order/AddOrder",
            new StringContent(JsonConvert.SerializeObject(orderRequest), Encoding.UTF8, "application/json"));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, orderResponse.StatusCode);
    }

    void IDisposable.Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}