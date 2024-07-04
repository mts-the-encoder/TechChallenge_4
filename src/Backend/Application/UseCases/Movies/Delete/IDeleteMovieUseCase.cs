namespace Application.UseCases.Movies.Delete;

public interface IDeleteMovieUseCase
{
    Task Execute(long id);
}