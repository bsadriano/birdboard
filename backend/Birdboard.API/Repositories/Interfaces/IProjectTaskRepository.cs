using Birdboard.API.Dtos.ProjectTask;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IProjectTaskRepository : IRepository<ProjectTask, UpdateProjectTaskRequestDto>
{

}
