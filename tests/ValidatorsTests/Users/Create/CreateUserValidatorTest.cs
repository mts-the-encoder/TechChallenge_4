using Application.UseCases.User.Create;
using Exceptions;
using FluentAssertions;
using Utils.Requests;
using Xunit;

namespace Validators.Tests.Users.Create;

public class CreateUserValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Error_Blank_Name()
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Email já está registrado");
    }

    [Fact]
    public void Validate_Error_Blank_Email()
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Email não pode ser vazio");
    }

    [Fact]
    public void Validate_Error_Invalid_Email()
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build();
        request.Email = "email@";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Email inválido");
    }

    [Fact]
    public void Validate_Error_Blank_Password()
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Senha não pode estar em branco");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Error_Short_Password(int passwordLength)
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == "Senha deve ter 6 caracteres");
    }

    [Fact]
    public void Validate_Error_Invalid_Phone()
    {
        var validator = new CreateUSerValidator();

        var request = UserRequestBuilder.Build();
        request.YearBorn = "01 0 0101-0101";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}