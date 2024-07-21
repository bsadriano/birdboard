namespace Birdboard.API.Models;

public class Project
{
    public int Id { get; set; }
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string Path() => $"/api/projects/{Id}";
}
