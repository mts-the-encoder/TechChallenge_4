using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Movies.Update;

public interface IUpdateMovieUseCase
{
    Task<MovieResponse> Execute(long id, MovieRequest request);
}