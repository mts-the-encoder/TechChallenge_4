using Application.Services.UserSigned;
using AutoMapper;
using Communication.Responses;
using Domain.Repositories.Movies;
using Exceptions.ExceptionBase;

namespace Application.UseCases.Movies.GetAll;

public class GetAllMoviesUseCase : IGetAllMovieUseCase
{
    private readonly IMovieReadOnlyRepository _repository;
    private readonly IUserSigned _userSigned;
    private readonly IMapper _mapper;
    public GetAllMoviesUseCase(IMovieReadOnlyRepository repository, IUserSigned userSigned, IMapper mapper)
    {
        _repository = repository;
        _userSigned = userSigned;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MovieResponse>> Execute()
    {
        var userSigned = await _userSigned.GetUser();

        var movies = await _repository.GetAll(userSigned.Id);

        Validate(userSigned, movies);

        return _mapper.Map<IEnumerable<MovieResponse>>(movies);
    }

    private static void Validate(Domain.Entities.User user, IList<Domain.Entities.Movie> movie)
    {
        if (movie.Any(x => x.UserId != user.Id) || movie is null)
            throw new ValidationErrorsException(new List<string> { "Não encontrado" });
    }
}
