using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.User.Create;

public interface ICreateUserUseCase
{
    Task<UserResponse> Execute(UserRequest request);
}