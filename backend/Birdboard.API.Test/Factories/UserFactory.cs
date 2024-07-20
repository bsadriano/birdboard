using Birdboard.API.Data;
using Birdboard.API.Dtos.AppUser;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using Bogus;

namespace Birdboard.API.Test.Factories;

public class UserFactory
{
    public List<RegisterUserDto> GetUsers(int count, bool useNewSeed = false)
    {
        return GetUserFaker(useNewSeed).Generate(count);
    }

    public RegisterUserDto GetUser(bool useNewSeed = false)
    {
        return GetUsers(1, useNewSeed)[0];
    }

    public async Task<AppUser> Create(BirdboardDbContext dbContext)
    {
        var userDto = GetUser();
        JwtTokenHelper.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        AppUser user = new AppUser
        {
            Email = userDto.Email,
            UserName = userDto.UserName,
            PasswordHash = Convert.ToBase64String(passwordHash),
            PasswordSalt = passwordSalt
        };
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    private Faker<RegisterUserDto> GetUserFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }

        var faker = new Faker<RegisterUserDto>()
            .RuleFor(t => t.UserName, (faker, t) => faker.Internet.UserName())
            .RuleFor(t => t.Email, (faker, t) => faker.Internet.Email())
            .RuleFor(t => t.Password, (faker, t) => faker.Internet.Password());

        return faker.UseSeed(seed);
    }
}
