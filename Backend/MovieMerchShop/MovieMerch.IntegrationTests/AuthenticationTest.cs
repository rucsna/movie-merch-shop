using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Contracts;
using MovieMerchShop.Data;
using MovieMerchShop.Model;
using MovieMerchShop.Service.Authentication;
using MovieMerchShop.Service.Repository;
using Newtonsoft.Json;

namespace MovieMerch.IntegrationTests;

public class AuthenticationTest : IDisposable
{
    private readonly MovieMerchFactory _factory;
    private readonly HttpClient _client;
    //private readonly IUserRepository _userRepository;

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

    // [Fact]
    // public async Task Test_Registration_ReturnsOk_WhenSuccessfulRegistration()
    // {
    //     // Registration Attempt
    //     var registrationRequest = new RegistrationRequest("test1@test.com", "tester1", "testPassword",
    //         new DateTime(2000, 12, 12), "testAddress");
    //     var registrationResponse = await _client.PostAsync("api/Auth/Register",
    //         new StringContent(JsonConvert.SerializeObject(registrationRequest), Encoding.UTF8, "application/json"));
    //
    //     // Assert
    //     Assert.Equal(HttpStatusCode.Created, registrationResponse.StatusCode);
    //
    //     //IUserRepository userRepository = new UserRepository(new UsersContext(new DbContextOptions<UsersContext>()));
    //     var userToDelete = await userRepository.GetUserByEmailAsync(registrationRequest.Email);
    //     await userRepository.DeleteUserAsync(userToDelete);
    // }


    void IDisposable.Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}