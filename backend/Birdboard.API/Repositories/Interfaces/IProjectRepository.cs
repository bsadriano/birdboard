using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<ProjectDto>> GetAllAsync(string userId);
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(Project model);
    Task<ProjectDto?> UpdateAsync(int id, UpdateProjectRequestDto model);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsMember(int projectId, string userId);

}
