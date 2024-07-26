
namespace Birdboard.API.Models;

public class Activity : BaseEntity
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int SubjectId { get; set; }
    public string SubjectType { get; set; }
}
