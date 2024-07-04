using Application.Services.UserSigned;
using Moq;

namespace Utils.UserSigned;

public class UserSignedBuilder
{
    private static UserSignedBuilder _instance;
    private readonly Mock<IUserSigned> _repository;

    private UserSignedBuilder()
    {
        _repository ??= new Mock<IUserSigned>();
    }

    public static UserSignedBuilder Instance()
    {
        _instance = new UserSignedBuilder();
        return _instance;
    }

    public UserSignedBuilder GetUser(Domain.Entities.User user)
    {
        _repository.Setup(x => x.GetUser()).ReturnsAsync(user);
        return this;
    }

    public IUserSigned Build()
    {
        return _repository.Object;
    }
}