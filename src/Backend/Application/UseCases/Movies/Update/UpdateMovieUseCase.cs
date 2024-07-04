using Application.Services.UserSigned;
using Application.UseCases.Movies.Create;
using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Repositories.Movies;
using Exceptions;
using Exceptions.ExceptionBase;
using Infrastructure.RepositoryAccess.UnitOfWork;
using Serilog;

namespace Application.UseCases.Movies.Update;

public class UpdateMovieUseCase : IUpdateMovieUseCase
{
    private readonly IMovieUpdateOnlyRepository _repository;
    private readonly IMovieReadOnlyRepository _readOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUserSigned _userSigned;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMovieUseCase(IMovieUpdateOnlyRepository repository, IMapper mapper, IUserSigned userSigned, IUnitOfWork unitOfWork, IMovieReadOnlyRepository readOnlyRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userSigned = userSigned;
        _unitOfWork = unitOfWork;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<MovieResponse> Execute(long id, MovieRequest request)
    {
        var userSigned = await _userSigned.GetUser();

        var movie = await _readOnlyRepository.GetById(id);

        Validate(userSigned, movie);

        _mapper.Map(request, movie);

		_repository.Update(movie);

        await _unitOfWork.Commit();

        return _mapper.Map<MovieResponse>(movie);
    }

    private void Validate(Domain.Entities.User user, Domain.Entities.Movie movie)
    {
        var request = _mapper.Map<MovieRequest>(movie);
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

        if (movie.UserId != user.Id || movie is null)
            throw new ValidationErrorsException(new List<string> { "Não encontrado" });
    }
}