using Application.Services.UserSigned;
using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Movies;
using Exceptions;
using Exceptions.ExceptionBase;

namespace Application.UseCases.Movies.GetById;

public class GetByIdMovieUseCase : IGetByMovieUseCase
{
    private readonly IMovieReadOnlyRepository _repository;
    private readonly IUserSigned _userSigned;
    private readonly IMapper _mapper;
    public GetByIdMovieUseCase(IMapper mapper, IUserSigned userSigned, IMovieReadOnlyRepository repository)
    {
        _mapper = mapper;
        _userSigned = userSigned;
        _repository = repository;
    }

    public async Task<MovieResponse> Execute(long id)
    {
        var userSigned = await _userSigned.GetUser();

        var movie = await _repository.GetById(id);

        Validate(userSigned, movie);

        return _mapper.Map<MovieResponse>(movie);
    }

    private static void Validate(Domain.Entities.User user, Domain.Entities.Movie movie)
    {
        if (movie.UserId != user.Id || movie is null)
            throw new ValidationErrorsException(new List<string> { "Não encontrado" });
    }
}