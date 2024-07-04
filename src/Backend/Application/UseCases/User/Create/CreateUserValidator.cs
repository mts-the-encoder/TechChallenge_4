using Communication.Requests;
using FluentValidation;

namespace Application.UseCases.User.Create;

public class CreateUSerValidator : AbstractValidator<UserRequest>
{
    public CreateUSerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Email já está registrado");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email não pode ser vazio");
        RuleFor(x => x.YearBorn).NotEmpty().WithMessage("Ano de nascimento não pode estar vazio");
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email inválido");
        });

		When(c => !string.IsNullOrWhiteSpace(c.YearBorn), () =>
		{
			RuleFor(c => c.YearBorn)
				.Must(age => int.TryParse(age, out var result) && result >= 18)
				.WithMessage("Você precisa ser maior de 18");
		});
	}
}