using Birdboard.API.Models;

namespace Birdboard.API.Services.TokenService;

public interface ITokenService
{
    public string CreateToken(AppUser user);
    public RefreshToken GenerateRefreshToken();
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}
