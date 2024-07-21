using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.Project;

public class CreateProjectTaskRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Body cannot be less than 3 characters")]
    [MaxLength(50, ErrorMessage = "Body cannot be over 50 characters")]
    public string Body { get; set; }
}
