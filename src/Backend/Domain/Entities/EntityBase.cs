using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class EntityBase
{
    [Key]
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}