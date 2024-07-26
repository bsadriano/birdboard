using Birdboard.API.Dtos.Activity;
using Birdboard.API.Models;

namespace Birdboard.API.Repositories.Interfaces;

public interface IActivityRepository
{
    public IEnumerable<Activity> GetAll();
    public IEnumerable<IRecordable> GetActivitiesForEntity(int entityId, string entityType);
}
