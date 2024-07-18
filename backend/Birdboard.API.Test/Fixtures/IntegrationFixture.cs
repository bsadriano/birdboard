using Birdboard.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using Testcontainers.MsSql;

namespace Birdboard.API.Test
{
    public class IntegrationFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;
        public MockApp App { get; set; }
        public HttpClient Client { get; set; }
        private Respawner _respawner = default!;

        public IntegrationFixture()
        {
            _msSqlContainer = new MsSqlBuilder().Build();
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_msSqlContainer.GetConnectionString());
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();
            App = new MockApp(_msSqlContainer.GetConnectionString());
            Client = App.CreateClient();
            _respawner = await Respawner.CreateAsync(_msSqlContainer.GetConnectionString());
        }

        public class MockApp : WebApplicationFactory<Program>
        {
            private readonly string _msSqlConnection;

            public MockApp(string msSqlConnection)
            {
                _msSqlConnection = msSqlConnection;
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                base.ConfigureWebHost(builder);
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<BirdboardDbContext>));
                    services.AddSqlServer<BirdboardDbContext>(_msSqlConnection);

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BirdboardDbContext>();
                    dbContext.Database.EnsureCreated();
                });
            }
        }
    }

    [CollectionDefinition(nameof(IntegrationFixtureCollection))]
    public class IntegrationFixtureCollection : ICollectionFixture<IntegrationFixture>
    {

    }

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
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            Scope.Dispose();
            await IntegrationFixture.ResetDatabaseAsync();
        }
    }
}
