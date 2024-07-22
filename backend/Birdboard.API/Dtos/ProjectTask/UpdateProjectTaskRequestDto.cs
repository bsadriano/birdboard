using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.ProjectTask;

public class UpdateProjectTaskRequestDto
{
    [MinLength(3, ErrorMessage = "Body cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Body cannot be over 50 characters")]
    public string? Body { get; set; }
    public bool? Completed { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
