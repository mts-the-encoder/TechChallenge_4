using Bogus;
using Communication.Enums;
using Communication.Requests;

namespace Utils.Requests;

public class MovieRequestBuilder
{
    public static MovieRequest Build()
    {
	    var movieCreated = new Faker<MovieRequest>()
		    .RuleFor(x => x.Name, f => f.Lorem.Words().ToString())
		    .RuleFor(x => x.Gender, f => f.PickRandom<Gender>())
		    .RuleFor(x => x.Country, f => f.PickRandom<Country>())
		    .RuleFor(x => x.Director, f => f.Person.FullName)
		    .RuleFor(x => x.Duration, f => f.Random.Int(1, 5).ToString())
		    .RuleFor(x => x.Rate, f => f.Random.Double(0.00, 10.00))
		    .RuleFor(x => x.ReleasedYear, f => f.Random.Int(1900, 2000).ToString());

		return movieCreated;
    }
}