using Newtonsoft.Json;
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
            ProjectId = activityModel.ProjectId,
            SubjectId = activityModel.SubjectId,
            SubjectType = activityModel.SubjectType,
            EntityData = JsonConvert.DeserializeObject(activityModel.EntityData),
            Changes = JsonConvert.DeserializeObject(activityModel.Changes ?? ""),
            CreatedAt = activityModel.CreatedAt,
        };
}
