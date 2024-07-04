using Communication.Responses;

namespace Application.UseCases.Movies.GetAll;

public interface IGetAllMovieUseCase
{
    Task<IEnumerable<MovieResponse>> Execute();
}