using Birdboard.API.Dtos.ProjectTask;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;
using Birdboard.API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Birdboard.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/tasks")]
[Authorize]
public class ProjectTaskController : ControllerBase
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;

    public ProjectTaskController(
        IProjectTaskRepository projectTaskRepository,
        IProjectRepository projectRepository,
        IUserService userService,
        UserManager<AppUser> userManager)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectTaskDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAll()
    {
        var projectTasks = await _projectTaskRepository.GetAllAsync();

        return Ok(
            projectTasks.Select(p => p.ToProjectTaskDto())
                        .ToList()
        );
    }

    [HttpGet]
    [Route("{taskId:int}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectTaskDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetById([FromRoute] int taskId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projectTask = await _projectTaskRepository.GetByIdAsync(taskId);

            if (projectTask == null)
                return NotFound();

            if (projectTask.Project.OwnerId != _userService.GetAuthId())
                return StatusCode(403);

            return Ok(projectTask.ToProjectTaskDto());
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }


    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ProjectTaskDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProjectTask([FromRoute] int projectId, [FromBody] CreateProjectTaskRequestDto createProjectTaskDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = await _projectRepository.GetByIdAsync(projectId);

            var isMember = await _projectRepository.IsMember(projectId, _userService.GetAuthId());

            if (project!.OwnerId != _userService.GetAuthId() && !isMember)
                return StatusCode(403);

            var appUser = await _userManager.FindByIdAsync(_userService.GetAuthId());

            if (appUser == null)
                return BadRequest("Logged in user not found");

            var projectTaskModel = createProjectTaskDto.ToProjectTask();
            project.UpdatedAt = DateTime.UtcNow;
            projectTaskModel.ProjectId = projectId;

            await _projectTaskRepository.CreateAsync(projectTaskModel);

            return CreatedAtAction(
                nameof(GetById),
                new { projectId, taskId = projectTaskModel.Id },
                projectTaskModel.ToProjectTaskDto()
            );
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPatch]
    [Route("{taskId:int}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectTaskDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateProjectTask(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] UpdateProjectTaskRequestDto updateProjectTaskDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var project = await _projectRepository.GetByIdAsync(projectId);

        var isMember = await _projectRepository.IsMember(projectId, _userService.GetAuthId());

        if (project!.OwnerId != _userService.GetAuthId() && !isMember)
            return StatusCode(403);

        var projectTask = await _projectTaskRepository.UpdateAsync(taskId, updateProjectTaskDto);

        if (projectTask == null)
            return NotFound();

        return Ok(projectTask.ToProjectTaskDto());
    }
}
