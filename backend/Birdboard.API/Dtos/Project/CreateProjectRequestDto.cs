using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.Project;

public class CreateProjectRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Project cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Project cannot be over 50 characters")]
    public string Title { get; set; }
    [MinLength(3, ErrorMessage = "Description cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Description cannot be over 50 characters")]
    public string Description { get; set; }
}
