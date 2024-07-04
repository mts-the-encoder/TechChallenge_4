using Application.UseCases.User.Create;
using Application.UseCases.User.UpdatePassword;
using Communication.Requests;
using Exceptions;
using FluentAssertions;
using Utils.Requests;
using Xunit;

namespace Validators.Tests.Users.UpdatePassword;

public class UpdatePasswordValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new UpdatePasswordValidator();

        var request = UpdatePasswordRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().Be(true);
        request.NewPassword.Length.Should().BeGreaterOrEqualTo(6);
        request.Should().NotBeNull();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Error_Invalid_Password(int passwordLength)
    {
        var validator = new UpdatePasswordValidator();

        var request = UpdatePasswordRequestBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Senha deve ter 6 caracteres");
        request.NewPassword.Length.Should().BeLessOrEqualTo(5);
    }

    [Fact]
    public void Validate_Error_Blank_Password()
    {
        var validator = new UpdatePasswordValidator();

        var request = UpdatePasswordRequestBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Senha não pode estar em branco");
    }
}