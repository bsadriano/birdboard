using Birdboard.API.Dtos.Activity;
using Birdboard.API.Models;

namespace Birdboard.API.Mappers;

public static class ActivityMapper
{
    public static ActivityDto ToActivityDto(this Activity activityModel) =>
        new ActivityDto
        {
            Id = activityModel.Id,
            Description = activityModel.Description,
            SubjectId = activityModel.SubjectId,
            SubjectType = activityModel.SubjectType,
            CreatedAt = activityModel.CreatedAt,
        };
}
