using Birdboard.API.Models;

namespace Birdboard.API.Services.UserService;

public interface IUserService
{
    public string? GetId();
    public string? GetUserName();
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}
