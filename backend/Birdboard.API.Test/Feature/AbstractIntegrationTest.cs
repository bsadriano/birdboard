using System.Net.Http.Headers;
using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test.Feature;

[Collection(nameof(IntegrationFixtureCollection))]
public abstract class AbstractIntegrationTest : IAsyncLifetime
{
    public IntegrationFixture IntegrationFixture { get; }
    public HttpClient Client => IntegrationFixture.Client;
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;
    public BirdboardDbContext DbContext => Services.GetRequiredService<BirdboardDbContext>();
    public UserFactory _userFactory { get; set; }
    public ProjectFactory _projectFactory { get; set; }
    public ProjectTaskFactory _projectTaskFactory { get; set; }

    public AbstractIntegrationTest(IntegrationFixture integrationFixture)
    {
        IntegrationFixture = integrationFixture;
    }

    public virtual Task InitializeAsync()
    {
        Scope = IntegrationFixture.App.Services.CreateScope();
        Client.DefaultRequestHeaders.Authorization = null;
        _userFactory = new UserFactory(DbContext);
        _projectFactory = new ProjectFactory(DbContext);
        _projectTaskFactory = new ProjectTaskFactory(DbContext);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await IntegrationFixture.ResetDatabaseAsync();
    }

    protected async Task SignIn(AppUser? appUser = null)
    {
        var user = appUser;

        if (user == null)
            user = await _userFactory.Create(true);

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
