using Birdboard.API.Data;
using Birdboard.API.Dtos.ProjectTask;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Birdboard.API.Repositories
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly BirdboardDbContext _context;

        public ProjectTaskRepository(BirdboardDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask> CreateAsync(ProjectTask model)
        {
            await _context.ProjectTasks.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<ProjectTask>> GetAllAsync()
        {
            return await _context.ProjectTasks
                .Include(p => p.Project)
                .ThenInclude(p => p.Owner)
                .ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(int id)
        {
            return await _context.ProjectTasks
                .Include(p => p.Project)
                .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ProjectTask?> UpdateAsync(int id, UpdateProjectTaskRequestDto projectTaskDto)
        {
            var projectTask = await GetByIdAsync(id);

            if (projectTask == null)
                return null;

            projectTask.Project.UpdatedAt = DateTime.UtcNow;

            if (projectTaskDto.Body is not null)
                projectTask.Body = projectTaskDto.Body;
            if (projectTaskDto.Completed is not null)
                projectTask.Completed = (bool)projectTaskDto.Completed;
            if (projectTaskDto.UpdatedAt is not null)
                projectTask.UpdatedAt = (DateTime)projectTaskDto.UpdatedAt;

            await _context.SaveChangesAsync();
            return projectTask;
        }
    }
}
