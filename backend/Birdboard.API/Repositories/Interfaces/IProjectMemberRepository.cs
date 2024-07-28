
using Birdboard.API.Dtos.Projectmember;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IProjectMemberRepository
{
    Task<ProjectMemberDto> CreateAsync(ProjectMember model);
}
