using Domain.Repositories.Movies;
using Moq;

namespace Utils.Repositories.Movies;

public class MovieReadOnlyRepositoryBuilder
{
    private static MovieReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IMovieReadOnlyRepository> _repository;

    private MovieReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IMovieReadOnlyRepository>();
    }

    public static MovieReadOnlyRepositoryBuilder Instance()
    {
        _instance = new MovieReadOnlyRepositoryBuilder();
        return _instance;
    }

    public IMovieReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}