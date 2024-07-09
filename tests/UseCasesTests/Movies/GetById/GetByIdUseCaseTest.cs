using Application.UseCases.Movies.GetById;
using Communication.Enums;
using Domain.Entities;
using Exceptions.ExceptionBase;
using FluentAssertions;
using Utils.Entities;
using Utils.Mapper;
using Utils.Repositories.Movies;
using Utils.UserSigned;
using Xunit;

namespace UseCases.Tests.Movies.GetById;

public class GetByIdUseCaseTest
{

	[Fact]
	public async Task Validate_Success()
	{
		var (user, _) = UserBuilder.Build();

		var movie = MovieBuilder.Build(user);

		var useCase = CreateUseCase(user, movie);

		var response = await useCase.Execute(movie.Id);

		response.Director.Should().Be(movie.Director);
		response.Gender.Should().Be((Gender)movie.Gender);
		response.Name.Should().Be(movie.Name);
		response.Rate.Should().Be(movie.Rate);
		response.ReleasedYear.Should().BeEquivalentTo(movie.ReleasedYear);
	}

	[Fact]
	public async Task Validate_Error_Not_Found()
	{
		var (user, _) = UserBuilder.Build();
		var (user2, _) = UserBuilder.Build2();

		var movie = MovieBuilder.Build(user2);

		var useCase = CreateUseCase(user, movie);

		var action = async () => { await useCase.Execute(movie.Id); };

		await action.Should().ThrowAsync<ValidationErrorsException>();
	}

	private static GetByIdMovieUseCase CreateUseCase(Domain.Entities.User user, Movie movie)
	{
		var userSigned = UserSignedBuilder.Instance().GetUser(user).Build();
		var mapper = MapperBuilder.Instance();
		var readRepo = MovieReadOnlyRepositoryBuilder.Instance().GetById(movie).Build();

		return new GetByIdMovieUseCase(mapper, userSigned, readRepo);
	}
}