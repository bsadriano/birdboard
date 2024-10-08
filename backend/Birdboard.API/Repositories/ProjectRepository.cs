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
            projectWithActivitiesMembers(project, activities)
        ).ToList();

        return projectsWithComments;
    }

    public async Task<List<ProjectDto>> GetAccessibleProjectsAsync(string userId)
    {
        // Retrieve project IDs from both project members and owned projects
        var projectIds = await _context.ProjectMembers
            .Where(pm => pm.UserId == userId)
            .Select(pm => pm.ProjectId)
            .Union(_context.Projects
                .Where(p => p.OwnerId == userId)
                .Select(p => p.Id))
            .ToListAsync();

        // Retrieve activities related to the projects
        var activities = await _context.Activities
            .Include(a => a.User)
            .Where(a => (a.SubjectType == "Project" && projectIds.Contains(a.SubjectId)) || projectIds.Contains(a.ProjectId))
            .ToListAsync();

        // Retrieve projects with their activities
        var memberProjects = await _context.ProjectMembers
            .Where(pm => pm.UserId == userId)
            .Include(pm => pm.Project)
                .ThenInclude(p => p.Owner)
            .Include(pm => pm.Project)
                .ThenInclude(p => p.Tasks)
            .Include(pm => pm.Project)
                .ThenInclude(p => p.Members)
                    .ThenInclude(pm => pm.User)
            .Select(pm => pm.Project)
            .ToListAsync();

        var ownedProjects = await _context.Projects
            .Where(p => p.OwnerId == userId)
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .Include(p => p.Members)
                .ThenInclude(m => m.User)
            .ToListAsync();

        var combinedProjects = memberProjects
            .Concat(ownedProjects)
            .DistinctBy(p => p.Id) // Assuming `Id` is the unique identifier
            .OrderByDescending(p => p.UpdatedAt)
            .ToList();

        return combinedProjects.Select(project =>
            projectWithActivitiesMembers(project, activities)
        ).ToList();
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .Include(p => p.Members)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (project is null)
            return null;

        var activities = await _context.Activities
            .Where(a => (a.SubjectType == "Project" && a.SubjectId == project.Id) || a.ProjectId == project.Id)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return projectWithActivitiesMembers(project, activities);
    }

    private ProjectDto projectWithActivitiesMembers(Project project, List<Activity> activities)
    {
        var projectDto = project.ToProjectDto();
        projectDto.Activities = activities
            .Select(a => a.ToActivityDto()).ToList();
        projectDto.Members = project.Members
            .Select(m => m.User.ToAppUserDto()).ToList();

        return projectDto;
    }

    private ProjectDto projectWithMembers(ProjectDto projectDto, List<AppUser> members)
    {
        projectDto.Members = members
            .Select(u => u.ToAppUserDto()).ToList();

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

        var members = _context.ProjectMembers.Where(pm => pm.ProjectId == id);
        _context.ProjectMembers.RemoveRange(members);

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
