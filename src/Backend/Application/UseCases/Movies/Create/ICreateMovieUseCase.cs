using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Movies.Create;

public interface ICreateMovieUseCase
{
    Task<MovieResponse> Execute(MovieRequest request);
}