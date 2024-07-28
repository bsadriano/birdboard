using Birdboard.API.Dtos.Projectmember;
using Birdboard.API.Models;

namespace Birdboard.API.Mappers;

public static class ProjectMemberMappers
{
    public static ProjectMemberDto ToProjectMemberDto(this ProjectMember projectMember) =>
        new ProjectMemberDto
        {
            Id = projectMember.Id,
            ProjectId = projectMember.ProjectId,
            UserId = projectMember.UserId,
            Project = projectMember.Project.ToProjectDto(),
            User = projectMember.User.ToAppUserDto()
        };
}
