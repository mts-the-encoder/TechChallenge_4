using Communication.Requests;
using Communication.Responses;

namespace Application.UseCases.Login.DoLogin;

public interface ILoginUseCase
{
    Task<LoginResponse> Execute(LoginRequest request);
}