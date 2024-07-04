using Exceptions;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Utils.Requests;
using Xunit;

namespace WebApi.Tests.V1.User.Create;

public class CreateUserTest : ControllerBase
{
    private const string METHOD = "user";

    public CreateUserTest(WebAppFactory<Program> factory) : base (factory)
    {
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = UserRequestBuilder.Build();

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Error_Empty_Name()
    {
        var request = UserRequestBuilder.Build();
        request.Name = string.Empty;

        var response = await PostRequest(METHOD,request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(x => x.GetString().Equals("Email já está registrado"));
    }

    [Fact]
    public async Task Validate_Error_Empty_Email()
    {
        var request = UserRequestBuilder.Build();
        request.Email = string.Empty;

        var response = await PostRequest(METHOD,request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("messages").EnumerateArray();
        errors.Should().ContainSingle().And.Contain(x => x.GetString().Equals("Email não pode ser vazio"));
    }
}