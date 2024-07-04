using Application.UseCases.Movies.Create;
using FluentAssertions;
using Utils.Requests;
using Xunit;

namespace Validators.Tests.Movies.Create;

public class CreateMovieValidatorTest
{
    [Fact]
    public void Validate_Success()
    {
        var validator = new MovieValidator();

        var request = MovieRequestBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Error_Blank_Country()
    {
        var validator = new MovieValidator();

        var request = MovieRequestBuilder.Build();
        request.Country = Communication.Enums.Country.BRA;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.00)]
    [InlineData(10.50)]
    [InlineData(25.00)]
    [InlineData(30.00)]
    [InlineData(35.00)]
    [InlineData(40.00)]
    [InlineData(45.00)]
    public void Validate_Error_Minimum_Rate(double rate)
    {
        var validator = new MovieValidator();

        var request = MovieRequestBuilder.Build();
        request.Rate = rate;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
	[InlineData(10.50)]
	[InlineData(25.00)]
	[InlineData(30.00)]
	[InlineData(35.00)]
	[InlineData(40.00)]
	[InlineData(45.00)]
    public void Validate_Error_Maximum_Rate(double rate)
    {
	    var validator = new MovieValidator();

	    var request = MovieRequestBuilder.Build();
	    request.Rate = rate;

	    var result = validator.Validate(request);

	    result.IsValid.Should().BeFalse();
    }
}