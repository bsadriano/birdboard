using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;

namespace Birdboard.API.Mappers;

public static class ProjectMappers
{
    public static ProjectDto ToProjectDto(this Project projectModel) =>
        new ProjectDto
        {
            Id = projectModel.Id,
            Title = projectModel.Title,
            Description = projectModel.Description,
            Path = projectModel.Path(),
            CreatedAt = projectModel.CreatedAt,
            UpdatedAt = projectModel.UpdatedAt,
            Tasks = projectModel.Tasks.Select(t => t.ToProjectTaskDto()).ToList()
        };

    public static Project ToProject(this CreateProjectRequestDto projectDto) =>
        new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
        };

    public static CreateProjectRequestDto ToCreateProjectRequestDto(this Project projectModel) =>
        new CreateProjectRequestDto
        {
            Title = projectModel.Title,
            Description = projectModel.Description
        };
}
