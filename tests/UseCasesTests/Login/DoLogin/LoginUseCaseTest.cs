using Application.UseCases.Login.DoLogin;
using Communication.Requests;
using Communication.Responses;
using Exceptions;
using Exceptions.ExceptionBase;
using FluentAssertions;
using Utils.Entities;
using Utils.Mapper;
using Utils.PasswordEncryptor;
using Utils.Repositories.User;
using Utils.Repositories;
using Utils.Token;
using Xunit;

namespace UseCases.Tests.Login.DoLogin;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var response = await useCase.Execute(new LoginRequest
        {
            Email = user.Email,
            Password = password
        });

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Error_Invalid_Password()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var action = async () =>
        {
            await useCase.Execute(new LoginRequest
            {
                Email = user.Email,
                Password = "password"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(ex => ex.Message.Equals("Login inválido"));
    }

    [Fact]
    public async Task Validate_Error_Invalid_Email()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var action = async () =>
        {
            await useCase.Execute(new LoginRequest
            {
                Email = "email@",
                Password = password
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(ex => ex.Message.Equals("Login inválido"));
    }

    [Fact]
    public async Task Validate_Error_Invalid_Email_And_Password()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var action = async () =>
        {
            await useCase.Execute(new LoginRequest
            {
                Email = "email@",
                Password = "pass"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(ex => ex.Message.Equals("Login inválido"));
    }

    private static LoginUseCase CreateUseCase(Domain.Entities.User user)
    {
        var repository = UserReadOnlyRepositoryBuilder.Instance().GetByEmailAndPassword(user).Build();
        var encryptor = PasswordEncryptorBuilder.Instance();
        var token = TokenServiceBuilder.Instance();

        return new LoginUseCase(repository, encryptor, token);
    }
}