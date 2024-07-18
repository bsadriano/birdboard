using Birdboard.API.Controllers.Mappers;
using Birdboard.API.Data;
using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Birdboard.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
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

        var projectModel = createProjectDto.ToProject();
        await _projectRepository.CreateAsync(projectModel);

        return CreatedAtAction(
            nameof(GetById),
            new { id = projectModel.Id },
            projectModel.ToProjectDto()
        );
    }
}
