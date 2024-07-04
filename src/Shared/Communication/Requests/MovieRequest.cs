using Communication.Enums;

namespace Communication.Requests;

public class MovieRequest
{
	public string Name { get; set; }
	public string Director { get; set; }
	public string ReleasedYear { get; set; }
	public string Duration { get; set; }
	public Country Country { get; set; }
	public Gender Gender { get; set; }
	public double Rate { get; set; }
}