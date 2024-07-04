using Domain.Entities;

namespace Domain.Repositories.Movies;

public interface IMovieUpdateOnlyRepository
{
    void Update(Movie movie);
}