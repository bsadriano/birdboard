using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Repositories.Interfaces;

namespace Birdboard.API.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly BirdboardDbContext _context;

    public ActivityRepository(BirdboardDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Activity> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IRecordable> GetActivitiesForEntity(int entityId, string entityType)
    {
        var activities = _context.Activities
            .Where(c => c.SubjectId == entityId && c.SubjectType == entityType)
            .ToList();

        var results = new List<IRecordable>();

        foreach (var activity in activities)
        {
            IRecordable recordableEntity = null;

            if (activity.SubjectType == "Project")
                recordableEntity = _context.Projects.Find(activity.SubjectId);

            if (activity.SubjectType == "ProjectTask")
                recordableEntity = _context.ProjectTasks.Find(activity.SubjectId);

            if (recordableEntity != null)
                results.Add(recordableEntity);
        }

        return results;
    }
}
