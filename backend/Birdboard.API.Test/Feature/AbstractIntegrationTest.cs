using System.Net.Http.Headers;
using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test.Feature;

[Collection(nameof(IntegrationFixtureCollection))]
public abstract class IntegrationTest : IAsyncLifetime
{
    public IntegrationFixture IntegrationFixture { get; }
    public HttpClient Client => IntegrationFixture.Client;
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;
    public BirdboardDbContext DbContext => Services.GetRequiredService<BirdboardDbContext>();
    public UserFactory _userFactory { get; set; }
    public ProjectFactory _projectFactory { get; set; }

    public IntegrationTest(IntegrationFixture integrationFixture)
    {
        IntegrationFixture = integrationFixture;
    }

    public Task InitializeAsync()
    {
        Scope = IntegrationFixture.App.Services.CreateScope();
        Client.DefaultRequestHeaders.Authorization = null;
        _userFactory = new UserFactory(DbContext);
        _projectFactory = new ProjectFactory(DbContext);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await IntegrationFixture.ResetDatabaseAsync();
    }

    protected void LoginAs(AppUser user)
    {
        var token = new TestJwtToken()
            .WithId(user.Id)
            .WithUserName(user.UserName)
            .WithRole("User")
            .Build();

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}



[CollectionDefinition(nameof(IntegrationFixtureCollection))]
public class IntegrationFixtureCollection : ICollectionFixture<IntegrationFixture>
{

}
