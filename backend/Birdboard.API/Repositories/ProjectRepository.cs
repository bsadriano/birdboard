using Birdboard.API.Data;
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
}
