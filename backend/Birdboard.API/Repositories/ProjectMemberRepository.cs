using Birdboard.API.Data;
using Birdboard.API.Dtos.Projectmember;
using Birdboard.API.Mappers;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public class ProjectMemberRepository : IProjectMemberRepository
{
    private readonly BirdboardDbContext _context;

    public ProjectMemberRepository(BirdboardDbContext context)
    {
        _context = context;
    }
    public async Task<ProjectMemberDto> CreateAsync(ProjectMember model)
    {
        await _context.AddAsync(model);
        await _context.SaveChangesAsync();

        return model.ToProjectMemberDto();
    }
}
