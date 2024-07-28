using Birdboard.API.Dtos.AppUser;
using Birdboard.API.Dtos.Project;

namespace Birdboard.API.Dtos.Projectmember;

public class ProjectMemberDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; }
    public ProjectDto Project { get; set; }
    public AppUserDto User { get; set; }
}
