using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using Bogus;

namespace Birdboard.API.Test.Factories;

public class UserFactory
{
    public BirdboardDbContext DbContext { get; set; }

    public UserFactory(BirdboardDbContext dbContext = null)
    {
        DbContext = dbContext;
    }

    public List<AppUser> GetUsers(int count, bool useNewSeed = false)
    {
        return GetUserFaker(useNewSeed).Generate(count);
    }

    public AppUser GetUser(bool useNewSeed = false)
    {
        return GetUsers(1, useNewSeed)[0];
    }

    public async Task<AppUser> Create(bool useNewSeed = false)
    {
        var newUser = GetUser(useNewSeed);
        AppUser user = new AppUser
        {
            Email = newUser.Email,
            UserName = newUser.UserName,
            PasswordHash = newUser.PasswordHash,
            PasswordSalt = newUser.PasswordSalt,
        };
        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();

        return user;
    }

    public Faker<AppUser> GetUserFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }

        JwtTokenHelper.CreatePasswordHash("123456", out byte[] passwordHash, out byte[] passwordSalt);
        var faker = new Faker<AppUser>()
            .RuleFor(t => t.UserName, (faker, t) => faker.Internet.UserName())
            .RuleFor(t => t.Email, (faker, t) => faker.Internet.Email())
            .RuleFor(t => t.PasswordHash, o => Convert.ToBase64String(passwordHash))
            .RuleFor(t => t.PasswordSalt, o => passwordSalt);

        return faker.UseSeed(seed);
    }
}
