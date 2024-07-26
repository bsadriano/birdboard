using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(Project model);
    Task<ProjectDto?> UpdateAsync(int id, UpdateProjectRequestDto model);
}
