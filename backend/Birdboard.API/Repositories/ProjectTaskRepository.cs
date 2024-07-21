using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Birdboard.API.Data;
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
    }
}
