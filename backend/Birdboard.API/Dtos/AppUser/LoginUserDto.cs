using System.ComponentModel.DataAnnotations;

namespace Birdboard.API.Dtos.AppUser;

public class LoginUserDto
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}
