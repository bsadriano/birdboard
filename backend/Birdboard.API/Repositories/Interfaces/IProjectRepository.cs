using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IProjectRepository : IRepository<Project, UpdateProjectRequestDto>
{
}
