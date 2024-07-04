using Application.UseCases.Movies.Delete;
using FluentAssertions;
using Utils.Entities;
using Utils.Repositories;
using Utils.Repositories.Movies;
using Utils.UserSigned;
using Xunit;

namespace UseCases.Tests.Movies.Delete;

public class DeleteUseCaseTest
{
    [Fact]
    public async Task Validate_Success()
    {
        var (user, _) = UserBuilder.Build();

        var movie = MovieBuilder.Build(user);

        var useCase = CreateUseCase(user);

        var action = async () => { await useCase.Execute(movie.Id); };

        await action.Should().NotThrowAsync();
    }

    private static DeleteMovieUseCase CreateUseCase(Domain.Entities.User user)
    {
        var userSigned = UserSignedBuilder.Instance().GetUser(user).Build();
        var writeOnlyRepository = MovieReadOnlyRepositoryBuilder.Instance().Build();
        var readOnlyRepository = MovieWriteOnlyRepositoryBuilder.Instance().Build();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();

        return new DeleteMovieUseCase(readOnlyRepository, writeOnlyRepository, unitOfWork, userSigned);
    }
}