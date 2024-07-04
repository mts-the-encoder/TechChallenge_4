using Domain.Entities;

namespace Domain.Repositories.Movies;

public interface IMovieWriteOnlyRepository
{
    Task Create(Movie movie);
    Task Delete(long id);
}