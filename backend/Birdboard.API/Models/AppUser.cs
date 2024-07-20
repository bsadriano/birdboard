using Microsoft.AspNetCore.Identity;

namespace Birdboard.API.Models;

public class AppUser : IdentityUser
{
    public List<Project> Projects { get; set; } = new List<Project>();
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
}
