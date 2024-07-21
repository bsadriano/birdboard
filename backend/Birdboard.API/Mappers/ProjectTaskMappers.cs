using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;

namespace Birdboard.API.Mappers;

public static class ProjectTaskMappers
{
    public static ProjectTaskDto ToProjectTaskDto(this ProjectTask projectTaskModel) =>
        new ProjectTaskDto
        {
            Id = projectTaskModel.Id,
            Body = projectTaskModel.Body,
            ProjectId = projectTaskModel.ProjectId,
            CreatedAt = projectTaskModel.CreatedAt,
            UpdatedAt = projectTaskModel.UpdatedAt,
        };

    public static ProjectTask ToProjectTask(this CreateProjectTaskRequestDto projectTaskDto) =>
        new ProjectTask
        {
            Body = projectTaskDto.Body
        };

    public static CreateProjectTaskRequestDto ToCreateProjectTaskRequestDto(this ProjectTask projectTaskModel) =>
        new CreateProjectTaskRequestDto
        {
            Body = projectTaskModel.Body,
        };
}
