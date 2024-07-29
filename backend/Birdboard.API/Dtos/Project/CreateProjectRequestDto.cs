using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.Project;

public class CreateProjectRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Title cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Title cannot be over 50 characters")]
    public string Title { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Description cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Description cannot be over 50 characters")]
    public string Description { get; set; }
    [MinLength(3, ErrorMessage = "Notes cannot be less than 3 characters")]
    [MaxLength(100, ErrorMessage = "Notes cannot be over 100 characters")]
    public string? Notes { get; set; }
}
