using Exceptions;
using FluentValidation;

namespace Application.UseCases.User;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password).NotEmpty().WithMessage("Senha não pode estar em branco");

        When(password => !string.IsNullOrWhiteSpace(password),() =>
        {
            RuleFor(password => password).MinimumLength(6).WithMessage("Senha deve ter 6 caracteres");
        });
    }
}