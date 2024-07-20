using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Birdboard.API.Models;

namespace Birdboard.API.Services.UserService;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetId()
    {
        var result = string.Empty;

        if (_httpContextAccessor.HttpContext != null)
        {
            result = _httpContextAccessor
                .HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        return result;
    }

    public string? GetUserName()
    {
        var result = string.Empty;

        if (_httpContextAccessor.HttpContext != null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Upn);
        }
        return result;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
