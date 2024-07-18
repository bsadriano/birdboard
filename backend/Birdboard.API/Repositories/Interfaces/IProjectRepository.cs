using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(Project projectModel);
}
