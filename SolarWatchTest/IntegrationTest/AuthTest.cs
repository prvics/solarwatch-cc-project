using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using SolarWatch.Contracs;
using SolarWatch.Data;

namespace SolarWatchTest.IntegrationTests;

public class AuthTest
{
    private HttpClient _client;
    private SolarWatchWebApplicationFactory _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new SolarWatchWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        await CleanupTestUsers();
        _factory.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task Register_withValidData_ReturnsOk()
    {
        // Arrange
        const string testEmail = "test@user.com";
        const string testUsername = "test";
        const string testPassword = "test123";

        var request = new RegistrationRequest(testEmail, testUsername, testPassword);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Auth/Register", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<RegistrationResponse>(jsonResponse);

        result.Email.Should().Be(testEmail);
        result.UserName.Should().Be(testUsername);
    }

    [Test]
    public async Task Register_withShortPassword_ReturnsError()
    {
        // Arrange
        const string testEmail = "test@example.com";
        const string testUsername = "test";
        const string testPassword = "123";

        var request = new RegistrationRequest(testEmail, testUsername, testPassword);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Auth/Register", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task AuthController_Login_ReturnsOk()
    {
        // Arrange
        const string testEmail = "login@test.com";
        const string testPassword = "loginTest123";
        const string userName = "loginTester";

        // Pre-register the user
        await RegisterUser(testEmail, userName, testPassword, "User");

        var request = new AuthRequest(testEmail, testPassword);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Auth/Login", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<AuthResponse>(jsonResponse);

        result.Email.Should().Be(testEmail);
        result.UserName.Should().Be(userName);
        result.Token.Should().NotBeNullOrEmpty();


    }

    private async Task RegisterUser(string email, string username, string password, string role)
    {
        var request = new RegistrationRequest(email, username, password);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/Auth/Register", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to register user: {errorContent}");
        }
    }

    private async Task CleanupTestUsers()
    {
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var testUsers = userManager.Users.Where(u => u.Email.Contains("test@") || u.Email.Contains("@test")).ToList();

        foreach (var user in testUsers)
        {
            await userManager.DeleteAsync(user);
        }
    }
}