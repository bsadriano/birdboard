using Birdboard.API.Dtos.AppUser;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Services.TokenService;
using Birdboard.API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Birdboard.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<AppUser> userManager, IConfiguration configuration, IUserService userService, ITokenService tokenService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _userService = userService;
        _tokenService = tokenService;
    }


    [HttpGet, Authorize]
    public ActionResult<string> GetMe()
    {
        return Ok(_userService.GetAuthUserName());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var appUser = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = passwordSalt
            };

            var createdUser = await _userManager.CreateAsync(appUser);
            if (!createdUser.Succeeded)
                return StatusCode(500, createdUser.Errors);

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
                return StatusCode(500, roleResult.Errors);

            var token = _tokenService.CreateToken(appUser);

            return Ok(appUser.ToLoggedInUserDto(token));
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login(LoginUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(user => user.UserName == request.UserName);

        if (user == null)
            return Unauthorized("You have entered an invalid username or password");

        if (!_tokenService.VerifyPasswordHash(request.Password, Convert.FromBase64String(user.PasswordHash), user.PasswordSalt))
            return Unauthorized("You have entered an invalid username or password");

        string token = _tokenService.CreateToken(user);

        var refreshToken = _tokenService.GenerateRefreshToken();
        SetRefreshToken(refreshToken);

        return Ok(user.ToLoggedInUserDto(token));
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var user = await _userManager.Users
            .FirstOrDefaultAsync(user => user.Id == _userService.GetAuthId());

        if (!user.RefreshToken.Equals(refreshToken))
            return Unauthorized("Invalid Refresh Token");
        else if (user.TokenExpires < DateTime.Now)
            return Unauthorized("Token expired.");

        string token = _tokenService.CreateToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        SetRefreshToken(newRefreshToken);

        return Ok(token);
    }

    private async Task SetRefreshToken(RefreshToken newRefreshToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(user => user.Id == _userService.GetAuthId());

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
        await _userManager.UpdateAsync(user);
    }
}
