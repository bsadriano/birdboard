using Birdboard.API.Controllers.Mappers;
using Birdboard.API.Data;
using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Birdboard.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly BirdboardDbContext _context;

    public ProjectController(BirdboardDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
    [ProducesResponseType(400)]
    public IActionResult GetAll()
    {
        return Ok(_context.Projects.ToList());
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(200, Type = typeof(Project))]
    [ProducesResponseType(400)]
    public IActionResult GetById(int id)
    {
        return Ok(_context.Projects.Find(id));
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Project))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequestDto projectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var projectModel = projectDto.ToProject();
        await _context.Projects.AddAsync(projectModel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = projectModel.Id }, projectModel.ToProjectDto());
    }
}
