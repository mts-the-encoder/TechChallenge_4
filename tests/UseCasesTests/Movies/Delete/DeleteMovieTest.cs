using Application.UseCases.Movies.Delete;
using Domain.Entities;
using Exceptions.ExceptionBase;
using FluentAssertions;
using Utils.Entities;
using Utils.Repositories;
using Utils.Repositories.Movies;
using Utils.UserSigned;
using Xunit;

namespace UseCases.Tests.Movies.Delete;

public class DeleteMovieTest
{
	[Fact]
	public async Task Validate_Success()
	{
		var (user, _) = UserBuilder.Build();

		var movie = MovieBuilder.Build(user);

		var useCase = CreateUseCase(user, movie);

		var action = async () => { await useCase.Execute(movie.Id); };

		await action.Should().NotThrowAsync();
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

	private static DeleteMovieUseCase CreateUseCase(Domain.Entities.User user, Movie movie)
	{
		var userSigned = UserSignedBuilder.Instance().GetUser(user).Build();
		var writeRepo = MovieWriteOnlyRepositoryBuilder.Instance().Build();
		var readRepo = MovieReadOnlyRepositoryBuilder.Instance().GetById(movie).Build();
		var unitOfWork = UnitOfWorkBuilder.Instance().Build();

		return new DeleteMovieUseCase(writeRepo, readRepo, unitOfWork, userSigned);
	}
}
