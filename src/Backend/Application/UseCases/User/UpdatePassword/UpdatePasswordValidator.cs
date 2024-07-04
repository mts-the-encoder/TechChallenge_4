using Communication.Requests;
using FluentValidation;

namespace Application.UseCases.User.UpdatePassword;

public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequest>
{
    public UpdatePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator());
    }
}