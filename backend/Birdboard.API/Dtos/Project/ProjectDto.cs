using Birdboard.API.Dtos.Activity;
using Birdboard.API.Dtos.AppUser;
using Birdboard.API.Dtos.ProjectTask;

namespace Birdboard.API.Dtos.Project;

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public string OwnerId { get; set; }
    public string Path { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public AppUserDto Owner { get; set; }
    public List<ProjectTaskDto> Tasks { get; set; }
    public List<ActivityDto> Activities { get; set; }
    public List<AppUserDto>? Members { get; set; }
}
