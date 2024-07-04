using Communication.Responses;

namespace Application.UseCases.Movies.GetById;

public interface IGetByMovieUseCase
{
    Task<MovieResponse> Execute(long id);
}