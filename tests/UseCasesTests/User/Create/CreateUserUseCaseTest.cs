using Application.UseCases.User.Create;
using Exceptions;
using Exceptions.ExceptionBase;
using FluentAssertions;
using Utils.Mapper;
using Utils.PasswordEncryptor;
using Utils.Repositories;
using Utils.Repositories.User;
using Utils.Requests;
using Utils.Token;
using Xunit;

namespace UseCases.Tests.User.Create;

public class CreateUserUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        var request = UserRequestBuilder.Build();

        var useCase = CreateUseCase();

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Error_Email_Already_In_Use()
    {
        var request = UserRequestBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var action = async () => { await useCase.Execute(request); };

        await action.Should().ThrowAsync<ValidationErrorsException>();
    }

    [Fact]
    public async Task Validate_Error_Email_Blank()
    {
        var request = UserRequestBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        var action = async () => { await useCase.Execute(request); };

        await action.Should().ThrowAsync<ValidationErrorsException>();
    }

    private static CreateUserUseCase CreateUseCase(string email = "")
    {
        var repositoryWriteOnly = UserWriteOnlyRepositoryBuilder.Instance().Build();
        var mapper = MapperBuilder.Instance();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var encryptor = PasswordEncryptorBuilder.Instance();
        var token = TokenServiceBuilder.Instance();
        var repositoryReadOnly = UserReadOnlyRepositoryBuilder.Instance().ExistsByEmail(email).Build();

        return new CreateUserUseCase(repositoryWriteOnly, mapper, unitOfWork, encryptor, token, repositoryReadOnly);
    }
}