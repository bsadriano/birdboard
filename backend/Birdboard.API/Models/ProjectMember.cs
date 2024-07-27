using System.ComponentModel.DataAnnotations.Schema;

namespace Birdboard.API.Models;

[Table("ProjectMembers")]
public class ProjectMember : BaseEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; }
    public Project Project { get; set; }
    public AppUser User { get; set; }
}
