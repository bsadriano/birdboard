using Birdboard.API.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test;

public abstract class BaseIntegrationTest : IClassFixture<BirdboardWebApplicationFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _client;
    protected readonly BirdboardDbContext _dbContext;
    private readonly Func<Task> _resetDatabase;

    public BaseIntegrationTest(BirdboardWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<BirdboardDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;

        using (var scope = factory.Services.CreateScope())
        {
            var scopeService = scope.ServiceProvider;
            var dbContext = scopeService.GetRequiredService<BirdboardDbContext>();

            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
