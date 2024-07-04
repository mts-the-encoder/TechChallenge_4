using Communication.Requests;

namespace Application.UseCases.User.UpdatePassword;

public interface IUpdatePasswordUseCase
{
    Task Execute(UpdatePasswordRequest request);
}