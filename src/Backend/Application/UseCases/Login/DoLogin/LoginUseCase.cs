using Application.Services.Cryptography;
using Application.Services.Token;
using Communication.Requests;
using Communication.Responses;
using Domain.Repositories.User;
using Exceptions;
using Exceptions.ExceptionBase;
using Serilog;

namespace Application.UseCases.Login.DoLogin;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly Encryptor _encryptor;
    private readonly TokenService _tokenService;
    public LoginUseCase(IUserReadOnlyRepository repository, Encryptor encryptor, TokenService tokenService)
    {
        _repository = repository;
        _encryptor = encryptor;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Execute(LoginRequest request)
    {
        var password = _encryptor.Encrypt(request.Password);

        var user = await _repository.Login(request.Email, password);

        if (user is not null)
        {
            return new LoginResponse()
            {
                Name = user.Name,
                Token = _tokenService.GenerateToken(user.Email)
            };
        }
        Log.ForContext("UserName", request.Email)
            .Error($"{"Login inválido"}");
        throw new InvalidLoginException();
    }
}