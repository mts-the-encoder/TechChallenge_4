using System.Globalization;
using Communication.Requests;
using Exceptions;
using FluentMigrator.Infrastructure;
using FluentValidation;

namespace Application.UseCases.Movies.Create;

public class MovieValidator : AbstractValidator<MovieRequest>
{
    public MovieValidator()
    {
        RuleFor(x => x.Country).IsInEnum().WithMessage("Pais não pode ser vazio");
        RuleFor(x => x.Rate).LessThanOrEqualTo(10.0).GreaterThanOrEqualTo(0.0).WithMessage("Nota deve ser entre 0,0 e 10,0");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Nome é inválido");
        RuleFor(x => x.Director).NotEmpty().WithMessage("O diretor deve ser informado");
		RuleFor(x => x.ReleasedYear)
			.NotEmpty().WithMessage("Ano deve ser informado")
			.Must(BeValidYear).WithMessage("ANo de lançamento deve ser até 2000");
    }

    private bool BeValidYear(string yearString)
    {
	    if (!int.TryParse(yearString, out int year))
	    {
		    return false;
	    }

	    return year < 2000;
    }
}