using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Movies")]
public class Movie : EntityBase
{
	public string Name { get; set; }
	public string Director { get; set; }
	public string ReleasedYear { get; set; }
	public string Duration { get; set; }
	public Country Country { get; set; }
	public Gender Gender { get; set; } 
	public double Rate { get; set; }
	public long UserId { get; set; }
}