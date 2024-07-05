using System.Net;
using System.Text.Json;
using FluentAssertions;
using Utils.Requests;
using Xunit;

namespace WebApi.Tests.V1.Movies.Create;

public class CreateMovieTest : ControllerBase
{
    private const string METHOD = "movie";
    private Domain.Entities.User _user;
    private readonly string _password;

    public CreateMovieTest(WebAppFactory<Program> factory) : base(factory)
    {
        _user = factory.GetUser();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _password);

        var request = MovieRequestBuilder.Build();

        var response = await PostRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace();
    }
}