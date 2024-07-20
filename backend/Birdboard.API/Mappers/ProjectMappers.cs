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
            CreatedAt = projectModel.CreatedAt,
            UpdatedAt = projectModel.UpdatedAt,
        };

    public static Project ToProject(this CreateProjectRequestDto projectDto) =>
        new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
        };
}
