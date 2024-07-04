using Application.Services.Cryptography;
using Application.Services.Token;
using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Repositories.User;
using Exceptions.ExceptionBase;
using FluentValidation.Results;
using Infrastructure.RepositoryAccess.UnitOfWork;
using Serilog;

namespace Application.UseCases.User.Create;

public class CreateUserUseCase : ICreateUserUseCase
{
    private readonly IUserReadOnlyRepository _readRepository;
    private readonly IUserWriteOnlyRepository _writeRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Encryptor _encryptor;
    private readonly TokenService _tokenService;

    public CreateUserUseCase(IUserWriteOnlyRepository writeRepository, IMapper mapper, IUnitOfWork unitOfWork, Encryptor encryptor, TokenService tokenService, IUserReadOnlyRepository readRepository)
    {
        _writeRepository = writeRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _encryptor = encryptor;
        _tokenService = tokenService;
        _readRepository = readRepository;
    }

    public async Task<UserResponse> Execute(UserRequest request)
    {
        await Validate(request);

        var entity = _mapper.Map<Domain.Entities.User>(request);
        entity.Password = _encryptor.Encrypt(request.Password);

        await _writeRepository.Add(entity);

        await _unitOfWork.Commit();

        var token = _tokenService.GenerateToken(entity.Email);

        return new UserResponse()
        {
            Token = token
        };
    }

    private async Task Validate(UserRequest request)
    {
        var validator = new CreateUSerValidator();
        var result = await validator.ValidateAsync(request);

        var existsUser = await _readRepository.ExistsByEmail(request.Email);

        if (existsUser)
            result.Errors.Add(new ValidationFailure("email", "Email já está registrado"));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors
                .Select(error => error.ErrorMessage).ToList();

            var concatenatedErrors = string.Join("\n", errorMessages);

            Log.ForContext("UserName", request.Email)
                .Error($"{concatenatedErrors}");

            throw new ValidationErrorsException(errorMessages);
        }
    }
}