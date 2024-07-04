using Domain.Entities;

namespace Domain.Repositories.Movies;

public interface IMovieReadOnlyRepository
{
    Task<IList<Movie>> GetAll(long id);
    Task<Movie> GetById(long id);
}