namespace Birdboard.API.Dtos.ProjectTask;

public class ProjectTaskDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
