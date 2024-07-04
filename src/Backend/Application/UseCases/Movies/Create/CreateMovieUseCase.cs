using Application.Services.UserSigned;
using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Repositories.Movies;
using Exceptions.ExceptionBase;
using Infrastructure.RepositoryAccess.UnitOfWork;
using Serilog;

namespace Application.UseCases.Movies.Create;

public class CreateMovieUseCase : ICreateMovieUseCase
{
    private readonly IMovieWriteOnlyRepository _writeOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserSigned _userSigned;

    public CreateMovieUseCase(IMapper mapper, IMovieWriteOnlyRepository writeOnlyRepository, IUnitOfWork unitOfWork, IUserSigned userSigned)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
        _userSigned = userSigned;
        _mapper = mapper;
    }

    public async Task<MovieResponse> Execute(MovieRequest request)
    {
        Validate(request);

        var userSigned = await _userSigned.GetUser();

        var movie = _mapper.Map<Domain.Entities.Movie>(request);
        movie.UserId = (int)userSigned.Id;

        await _writeOnlyRepository.Create(movie);

        await _unitOfWork.Commit();

        return _mapper.Map<MovieResponse>(movie);
    }

    private void Validate(MovieRequest request)
    {
        var validator = new MovieValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors
                .Select(error => error.ErrorMessage).ToList();

            var concatenatedErrors = string.Join("\n", errorMessages);

            Log.ForContext("UserName", _userSigned.GetUser().Result.Email)
                .Error($"{concatenatedErrors}");

            throw new ValidationErrorsException(errorMessages);
        }
    }
}