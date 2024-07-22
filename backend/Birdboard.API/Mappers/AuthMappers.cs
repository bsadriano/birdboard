using Birdboard.API.Dtos.AppUser;
using Birdboard.API.Models;

namespace Birdboard.API.Mappers;

public static class AuthMappers
{
    public static LoggedinUserDto ToLoggedInUserDto(this AppUser user, string token) =>
        new LoggedinUserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        };
}
