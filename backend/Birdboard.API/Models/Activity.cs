
namespace Birdboard.API.Models;

public class Activity : BaseEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public string Description { get; set; }
    public int SubjectId { get; set; }
    public string SubjectType { get; set; }
    public string EntityData { get; set; }
    public string? Changes { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
}
