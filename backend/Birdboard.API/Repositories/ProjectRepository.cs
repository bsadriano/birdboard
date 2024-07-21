using Birdboard.API.Data;
using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Birdboard.API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly BirdboardDbContext _context;

    public ProjectRepository(BirdboardDbContext context)
    {
        _context = context;
    }

    public async Task<Project> CreateAsync(Project model)
    {
        await _context.Projects.AddAsync(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Project?> UpdateAsync(int id, UpdateProjectRequestDto model)
    {
        var project = await GetByIdAsync(id);

        if (project == null)
            return null;

        if (model.Title != null)
            project.Title = model.Title;
        if (model.Description != null)
            project.Description = model.Description;

        await _context.SaveChangesAsync();
        return project;
    }
}
