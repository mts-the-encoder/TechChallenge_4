using Application.UseCases.User.UpdatePassword;
using Communication.Requests;
using Exceptions;
using Exceptions.ExceptionBase;
using FluentAssertions;
using Utils.Entities;
using Utils.PasswordEncryptor;
using Utils.Repositories;
using Utils.Repositories.User;
using Utils.Requests;
using Utils.UserSigned;
using Xunit;

namespace UseCases.Tests.User.UpdatePassword;

public class UpdatePasswordUseCaseTest
{
    [Fact]
    private async Task Validate_Success()
    {
        var (user, password) = UserBuilder.Build();

        var request = UpdatePasswordRequestBuilder.Build();
        request.CurrentPassword = password;

        var useCase = CreateUseCase(user);

        var action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().NotThrowAsync();
    }

    [Fact]
    private async Task Validate_Error_New_Password_Blank()
    {
        var (user, password) = UserBuilder.Build();

        var request = UpdatePasswordRequestBuilder.Build();
        request.CurrentPassword = password;
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user);

        var action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().ThrowAsync<ValidationErrorsException>()
            .Where(x => x.ErrorMessages.Capacity == 1 && x.ErrorMessages
                .Contains("Senha não pode estar em branco"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    private async Task Validate_Error_Current_Password_Invalid(int passwordLength)
    {
        var (user, password) = UserBuilder.Build();

        var request = UpdatePasswordRequestBuilder.Build(passwordLength);
        request.CurrentPassword = password;

        var useCase = CreateUseCase(user);

        var action = async () =>
        {
            await useCase.Execute(request);
        };

        await action.Should().ThrowAsync<ValidationErrorsException>()
            .Where(x => x.ErrorMessages.Capacity == 1 && x.ErrorMessages
                .Contains("Senha deve ter 6 caracteres"));
    }

    private UpdatePasswordUseCase CreateUseCase(Domain.Entities.User user)
    {
        var encryptor = PasswordEncryptorBuilder.Instance();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var repository = UserUpdateOnlyRepositoryBuilder.Instance().GetById(user).Build();
        var userSigned = UserSignedBuilder.Instance().GetUser(user).Build();

        return new UpdatePasswordUseCase(repository, userSigned, encryptor, unitOfWork);
    }
}