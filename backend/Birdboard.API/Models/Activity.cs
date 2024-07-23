
namespace Birdboard.API.Models;

public class Activity : BaseEntity
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
