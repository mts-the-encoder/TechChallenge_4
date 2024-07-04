using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace Utils.Entities;

public class MovieBuilder
{
    public static Movie Build(User user)
    {
		var movie = new Faker<Movie>()
	        .RuleFor(x => x.Id, _ => user.Id)
            .RuleFor(x => x.Name, f => f.Lorem.Words().ToString())
            .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
			.RuleFor(x => x.Country, f => f.PickRandom<Country>())
            .RuleFor(x => x.Director, f => f.Person.FullName)
            .RuleFor(x => x.Duration, f => f.Random.Int(1, 5).ToString())
            .RuleFor(x => x.Rate, f => f.Random.Double(0.00, 10.00))
            .RuleFor(x => x.ReleasedYear, f => f.Random.Int(1900-2000).ToString())
            .RuleFor(x => x.UserId, _ => user.Id);

        return movie;
    }

}