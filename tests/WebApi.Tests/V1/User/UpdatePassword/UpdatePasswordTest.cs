using FluentAssertions;
using System.Net;
using System.Text.Json;
using Exceptions;
using Utils.Requests;
using Xunit;

namespace WebApi.Tests.V1.User.UpdatePassword;

public class UpdatePasswordTest : ControllerBase
{
    private const string METHOD = "user/update-password";
    private Domain.Entities.User _user;
    private readonly string _password;

    public UpdatePasswordTest(WebAppFactory<Program> factory) : base(factory)
    {
        _user = factory.GetUser();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _password);

        var request = UpdatePasswordRequestBuilder.Build();
        request.CurrentPassword = _password;

        var response = await PutRequest(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Validate_Unauthorized_User()
    {
        var request = UpdatePasswordRequestBuilder.Build();
        request.CurrentPassword = _password;
        request.NewPassword = string.Empty;

        var response = await PutRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}