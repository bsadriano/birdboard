namespace Birdboard.API.Models;

public class Project : BaseEntity, IRecordable
{
    public int Id { get; set; }
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Notes { get; set; }
    public List<Activity> Activities { get; set; } = new List<Activity>();
    public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    public List<ProjectMember> Members { get; set; } = new List<ProjectMember>();

    public string Path() => $"/api/projects/{Id}";
}
