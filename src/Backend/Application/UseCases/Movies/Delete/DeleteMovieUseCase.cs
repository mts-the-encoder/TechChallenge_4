using Application.Services.UserSigned;
using Domain.Repositories.Movies;
using Exceptions.ExceptionBase;
using Infrastructure.RepositoryAccess.UnitOfWork;

namespace Application.UseCases.Movies.Delete;

public class DeleteMovieUseCase : IDeleteMovieUseCase
{
    private readonly IMovieWriteOnlyRepository _writeOnlyRepository;
    private readonly IMovieReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserSigned _userSigned;
    public DeleteMovieUseCase(IMovieWriteOnlyRepository writeOnlyRepository, IMovieReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork, IUserSigned userSigned)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _userSigned = userSigned;
    }

    public async Task Execute(long id)
    {
        var userSigned = await _userSigned.GetUser();

        var movie = await _readOnlyRepository.GetById(id);

        Validate(userSigned, movie);

        await _writeOnlyRepository.Delete(id);

        await _unitOfWork.Commit();
    }

    private static void Validate(Domain.Entities.User user, Domain.Entities.Movie movie)
    {
        if (movie.UserId != user.Id || movie is null)
            throw new ValidationErrorsException(["Filme não encontrado"]);
    }
}