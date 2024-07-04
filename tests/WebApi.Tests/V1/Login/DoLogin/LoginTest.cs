using Communication.Requests;
using Exceptions;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Utils.Entities;
using Xunit;

namespace WebApi.Tests.V1.Login.DoLogin;

public class LoginTest : ControllerBase
{
    private const string METHOD = "login";
    private Domain.Entities.User _user;
    private string _password;

    public LoginTest(WebAppFactory<Program> factory) : base(factory)
    {
        _user = factory.GetUser();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var (user, password) = UserBuilder.Build();

        var request = new LoginRequest
        {
            Email = _user.Email,
            Password = _password,
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
            .And.Be(_user.Name);
    }

    [Fact]
    public async Task Validate_Error_Email()
    {
        var (user, password) = UserBuilder.Build();

        var request = new LoginRequest
        {
            Email = "mac@miller.com",
            Password = _password,
        };

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.Contain("Login inválido");
    }

    [Fact]
    public async Task Validate_Error_Password()
    {
        var (user, password) = UserBuilder.Build();

        var request = new LoginRequest
        {
            Email = _user.Email,
            Password = "_password",
        };

        var response = await PostRequest(METHOD,request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.Contain("Login inválido");
    }

    [Fact]
    public async Task Validate_Error_Email_And_Password()
    {
        var (user, password) = UserBuilder.Build();

        var request = new LoginRequest
        {
            Email = "mac@miller.com",
            Password = "_password",
        };

        var response = await PostRequest(METHOD,request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").Deserialize<List<string>>();
        errors.Should().ContainSingle().And.Contain("Login inválido");
    }
}