namespace Birdboard.API.Dtos.Activity;

public class ActivityDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Description { get; set; }
    public int SubjectId { get; set; }
    public string SubjectType { get; set; }
    public object EntityData { get; set; }
    public DateTime CreatedAt { get; set; }
}
