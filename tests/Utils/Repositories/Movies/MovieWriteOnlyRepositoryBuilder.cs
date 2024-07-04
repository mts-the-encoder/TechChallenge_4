using Domain.Repositories.Movies;
using Moq;

namespace Utils.Repositories.Movies;

public class MovieWriteOnlyRepositoryBuilder
{
    private static MovieWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IMovieWriteOnlyRepository> _repository;

    private MovieWriteOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IMovieWriteOnlyRepository>();
    }

    public static MovieWriteOnlyRepositoryBuilder Instance()
    {
        _instance = new MovieWriteOnlyRepositoryBuilder();
        return _instance;
    }

    public IMovieWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}