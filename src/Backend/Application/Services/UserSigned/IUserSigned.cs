using Domain.Entities;

namespace Application.Services.UserSigned;

public interface IUserSigned
{
    Task<User> GetUser();
}