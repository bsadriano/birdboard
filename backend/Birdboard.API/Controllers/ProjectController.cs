using Birdboard.API.Dtos.Project;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;
using Birdboard.API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Birdboard.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IProjectRepository _projectRepository;
    private readonly UserManager<AppUser> _userManager;

    public ProjectController(IUserService userService, IProjectRepository projectRepository, UserManager<AppUser> userManager)
    {
        _userService = userService;
        _projectRepository = projectRepository;
        _userManager = userManager;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectRepository.GetAllAsync();

        return Ok(
            projects.Select(p => p.ToProjectDto())
                    .ToList()
        );
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ProjectDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var project = await _projectRepository.GetByIdAsync(id);

        if (project.OwnerId != _userService.GetAuthId())
            return StatusCode(403);

        return project == null
            ? NotFound()
            : Ok(project.ToProjectDto());
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ProjectDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequestDto createProjectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var appUser = await _userManager.FindByIdAsync(_userService.GetAuthId());

        if (appUser == null)
        {
            return BadRequest("Logged in user not found");
        }

        var projectModel = createProjectDto.ToProject();
        projectModel.OwnerId = appUser.Id;
        await _projectRepository.CreateAsync(projectModel);

        return CreatedAtAction(
            nameof(GetById),
            new { id = projectModel.Id },
            projectModel.ToProjectDto()
        );
    }
}
