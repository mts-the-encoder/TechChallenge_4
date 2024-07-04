using Domain.Repositories.Movies;
using Moq;

namespace Utils.Repositories.Movies;

public class MovieUpdateOnlyRepositoryBuilder
{
    private static MovieUpdateOnlyRepositoryBuilder _instance;
    private readonly Mock<IMovieUpdateOnlyRepository> _repository;

    private MovieUpdateOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IMovieUpdateOnlyRepository>();
    }

    public static MovieUpdateOnlyRepositoryBuilder Instance()
    {
        _instance = new MovieUpdateOnlyRepositoryBuilder();
        return _instance;
    }

    public IMovieUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}