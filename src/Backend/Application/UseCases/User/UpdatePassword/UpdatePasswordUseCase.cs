using Application.Services.Cryptography;
using Application.Services.UserSigned;
using Communication.Requests;
using Domain.Repositories.User;
using Exceptions;
using Exceptions.ExceptionBase;
using FluentMigrator.Infrastructure;
using FluentValidation.Results;
using Infrastructure.RepositoryAccess.UnitOfWork;
using Serilog;

namespace Application.UseCases.User.UpdatePassword;

public class UpdatePasswordUseCase : IUpdatePasswordUseCase
{
    private readonly IUserSigned _userSigned;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly Encryptor _encryptor;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePasswordUseCase(IUserUpdateOnlyRepository repository, IUserSigned userSigned, Encryptor encryptor, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userSigned = userSigned;
        _encryptor = encryptor;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(UpdatePasswordRequest request)
    {
        var userSigned = await _userSigned.GetUser();

        var user = await _repository.GetById(userSigned.Id);

        Validate(request, user);

        user.Password = _encryptor.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    public void Validate(UpdatePasswordRequest request, Domain.Entities.User user)
    {
        var validator = new UpdatePasswordValidator();
        var result = validator.Validate(request);

        var currentPassword = _encryptor.Encrypt(request.CurrentPassword);

        if (!user.Password.Equals(currentPassword))
            result.Errors.Add(new ValidationFailure("currentPassword", "Senha inválida"));

        if (!result.IsValid)
        {
            var messages = result.Errors.Select(x => x.ErrorMessage).ToList();

            var concatenatedErrors = string.Join("\n", messages);

            Log.ForContext("UserName", user.Email)
                .Error($"{concatenatedErrors}");

            throw new ValidationErrorsException(messages);
        }
    }
}