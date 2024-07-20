using Birdboard.API.Models;

namespace Birdboard.API.Services.UserService;

public interface IUserService
{
    public string? GetAuthId();
    public string? GetAuthUserName();
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}
