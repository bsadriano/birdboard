using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.Project;

public class UpdateProjectRequestDto
{
    [MinLength(3, ErrorMessage = "Project cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Project cannot be over 50 characters")]
    public string? Title { get; set; }

    [MinLength(3, ErrorMessage = "Description cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Description cannot be over 50 characters")]
    public string? Description { get; set; }

    [MinLength(3, ErrorMessage = "Notes cannot be less than 3 characters")]
    [MaxLength(100, ErrorMessage = "Notes cannot be over 50 characters")]
    public string? Notes { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
