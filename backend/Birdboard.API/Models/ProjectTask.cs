namespace Birdboard.API.Models;

public class ProjectTask : BaseEntity
{
    public int Id { get; set; }
    public string Body { get; set; }
    public bool Completed { get; set; } = false;
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public string Path()
    {
        return $"/api/projects/{ProjectId}/tasks/{Id}";
    }
}
