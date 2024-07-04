using Communication.Enums;

namespace Communication.Responses;

public class MovieResponse
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string Director { get; set; }
	public string ReleasedYear { get; set; }
	public string Duration { get; set; }
	public Country Country { get; set; }
	public Gender Gender { get; set; }
	public double Rate { get; set; }
}