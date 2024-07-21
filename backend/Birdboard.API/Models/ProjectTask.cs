namespace Birdboard.API.Models;

public class ProjectTask
{
    public int Id { get; set; }
    public string Body { get; set; }
    public bool Completed { get; set; } = false;
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
