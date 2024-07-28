using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.ProjectInvitation;

public class ProjectInvitationRequestDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
