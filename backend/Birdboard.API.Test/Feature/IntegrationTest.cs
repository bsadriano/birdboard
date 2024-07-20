using System.Net.Http.Headers;
using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test.Feature;

[Collection(nameof(IntegrationFixtureCollection))]
public class IntegrationTest : IAsyncLifetime
{
    public IntegrationFixture IntegrationFixture { get; }
    public HttpClient Client => IntegrationFixture.Client;
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;
    public BirdboardDbContext DbContext => Services.GetRequiredService<BirdboardDbContext>();

    public IntegrationTest(IntegrationFixture integrationFixture)
    {
        IntegrationFixture = integrationFixture;
    }

    public Task InitializeAsync()
    {
        Scope = IntegrationFixture.App.Services.CreateScope();
        Client.DefaultRequestHeaders.Authorization = null;
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await IntegrationFixture.ResetDatabaseAsync();
    }

    protected async Task<AppUser> LoginAs(string userName, string password)
    {
        JwtTokenHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        AppUser user = new AppUser
        {
            UserName = userName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();

        var token = new TestJwtToken()
            .WithId(user.Id)
            .WithUserName(user.UserName)
            .WithRole("User")
            .Build();

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return user;
    }
}



[CollectionDefinition(nameof(IntegrationFixtureCollection))]
public class IntegrationFixtureCollection : ICollectionFixture<IntegrationFixture>
{

}
