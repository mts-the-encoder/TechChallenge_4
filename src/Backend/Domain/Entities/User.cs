using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Users")]
public class User : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string YearBorn { get; set; }
    public string Password { get; set; }    
}