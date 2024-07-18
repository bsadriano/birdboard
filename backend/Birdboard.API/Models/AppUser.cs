using Microsoft.AspNetCore.Identity;

namespace Birdboard.API.Models;

public class AppUser : IdentityUser
{
    public List<Project> Projects { get; set; } = new List<Project>();
}
