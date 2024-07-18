using Birdboard.API.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test;

public abstract class BaseIntegrationTest : IClassFixture<BirdboardWebApplicationFactory>
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _client;
    protected readonly BirdboardDbContext _dbContext;

    public BaseIntegrationTest(BirdboardWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<BirdboardDbContext>();
    }
}
