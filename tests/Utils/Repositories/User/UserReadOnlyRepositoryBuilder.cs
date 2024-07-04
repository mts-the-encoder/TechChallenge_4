using Domain.Repositories.User;
using Moq;

namespace Utils.Repositories.User;

public class UserReadOnlyRepositoryBuilder
{
    private static UserReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserReadOnlyRepository> _repository;

    private UserReadOnlyRepositoryBuilder()
    {
        _repository ??= new Mock<IUserReadOnlyRepository>();
    }

    public static UserReadOnlyRepositoryBuilder Instance()
    {
        _instance = new UserReadOnlyRepositoryBuilder();
        return _instance;
    }

    public UserReadOnlyRepositoryBuilder ExistsByEmail(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
            _repository.Setup(i => i.ExistsByEmail(email)).ReturnsAsync(true);
        
        return this;
    }

    public UserReadOnlyRepositoryBuilder GetByEmailAndPassword(Domain.Entities.User user)
    {
        _repository.Setup(x => x.Login(user.Email, user.Password))
            .ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}