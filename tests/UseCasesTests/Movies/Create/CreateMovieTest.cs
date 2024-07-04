using Application.UseCases.Movies.Create;
using FluentAssertions;
using Utils.Entities;
using Utils.Mapper;
using Utils.Repositories;
using Utils.Repositories.Movies;
using Utils.Requests;
using Utils.UserSigned;
using Xunit;

namespace UseCases.Tests.Movies.Create;

public class CreateMovieTest
{

    [Fact]
    public async Task Validate_Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = MovieRequestBuilder.Build();

        var useCase = CreateUseCase(user);

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Country.Should().Be(request.Country);
        response.Gender.Should().Be(request.Gender);
    }

    private static CreateMovieUseCase CreateUseCase(Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Instance();
        var unitOfWork = UnitOfWorkBuilder.Instance().Build();
        var repositoryWriteOnly = MovieWriteOnlyRepositoryBuilder.Instance().Build();
        var userSigned = UserSignedBuilder.Instance().GetUser(user).Build();

        return new CreateMovieUseCase(mapper, repositoryWriteOnly, unitOfWork, userSigned);
    }
}