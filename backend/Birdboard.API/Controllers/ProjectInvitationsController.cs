using Birdboard.API.Data;
using Birdboard.API.Dtos.ProjectInvitation;
using Birdboard.API.Dtos.Projectmember;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;
using Birdboard.API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Birdboard.API.Controllers;


[ApiController]
[Route("api/projects/{projectId:int}/invitations")]
[Authorize]
public class ProjectInvitationsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserService _userService;
    private readonly BirdboardDbContext _dbContext;

    public ProjectInvitationsController(
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository,
        UserManager<AppUser> userManager,
        IUserService userService,
        BirdboardDbContext dbContext)
    {
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
        _userManager = userManager;
        _userService = userService;
        _dbContext = dbContext;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ProjectMemberDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromRoute] int projectId, [FromBody] ProjectInvitationRequestDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == requestDto.Email);
        var project = await _dbContext.Projects.FindAsync(projectId);

        if (project!.OwnerId != _userService.GetAuthId())
            return StatusCode(403);

        if (user is null)
            return BadRequest(new
            {
                errors = new
                {
                    email = new string[] {
                        "The user you are inviting must have a Birdboard account"
                    }
                }
            });

        if (project is null)
            return BadRequest("Project not found");

        var projectMember = await _projectMemberRepository.CreateAsync(new ProjectMember
        {
            UserId = user.Id,
            ProjectId = projectId,
            User = user,
            Project = project
        });

        return Ok(projectMember);
    }
}
