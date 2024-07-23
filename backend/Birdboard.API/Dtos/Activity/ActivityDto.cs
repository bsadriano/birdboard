namespace Birdboard.API.Dtos.Activity;

public class ActivityDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}
