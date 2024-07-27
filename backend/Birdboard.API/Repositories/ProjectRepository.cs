using Birdboard.API.Data;
using Birdboard.API.Dtos.Project;
using Birdboard.API.Mappers;
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

    public async Task<ProjectDto> CreateAsync(Project model)
    {
        await _context.Projects.AddAsync(model);
        await _context.SaveChangesAsync();


        return await GetByIdAsync(model.Id);
    }

    public async Task<List<ProjectDto>> GetAllAsync(string userId)
    {
        var projects = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .OrderByDescending(p => p.UpdatedAt)
            .Where(p => p.OwnerId == userId)
            .ToListAsync();

        var projectIds = projects.Select(p => p.Id).ToList();
        var activities = await _context.Activities
            .Where(a => (a.SubjectType == "Project" && projectIds.Contains(a.SubjectId)) || projectIds.Contains(a.ProjectId))
            .ToListAsync();

        var projectsWithComments = projects.Select(project =>
            projectWithActivites(project, activities)
        ).ToList();

        return projectsWithComments;
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (project is null)
            return null;

        var activities = await _context.Activities
            .Where(a => (a.SubjectType == "Project" && a.SubjectId == project.Id) || a.ProjectId == project.Id)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return projectWithActivites(project, activities);
    }

    private ProjectDto projectWithActivites(Project project, List<Activity> activities)
    {
        var projectDto = project.ToProjectDto();
        projectDto.Activities = activities
            .Select(a => a.ToActivityDto()).ToList();

        return projectDto;
    }

    public async Task<ProjectDto?> UpdateAsync(int id, UpdateProjectRequestDto model)
    {
        var project = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (project == null)
            return null;

        if (model.Title is not null)
            project.Title = model.Title;
        if (model.Description is not null)
            project.Description = model.Description;
        if (model.UpdatedAt is not null)
            project.UpdatedAt = (DateTime)model.UpdatedAt;
        if (model.Notes is not null)
            project.Notes = model.Notes;

        await _context.SaveChangesAsync();

        return project.ToProjectDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (project == null)
            return false;

        _context.Projects.Remove(project);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsMember(int projectId, string userId)
    {
        var result = await _context.ProjectMembers
            .Where(pm => pm.UserId == userId && pm.ProjectId == projectId)
            .CountAsync();

        return result > 0;
    }
}
