using Birdboard.API.Data;
using Birdboard.API.Test.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
            await App.DisposeAsync();
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
                    SetupTestContainerDb(services, _msSqlConnection);

                    services.Configure<JwtBearerOptions>
                    (
                        JwtBearerDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.Configuration = new OpenIdConnectConfiguration
                            {
                                Issuer = JwtTokenProvider.Issuer,
                            };
                            options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                            options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                            options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                        }
                    );

                    static void SetupLocalDb(IServiceCollection services)
                    {
                        var connString = GetConnectionString();
                        services.AddSqlServer<BirdboardDbContext>(connString);

                        var dbContext = CreateDbContext(services);
                        dbContext.Database.EnsureDeleted();
                    }

                    static void SetupTestContainerDb(IServiceCollection services, string connString)
                    {
                        services.AddSqlServer<BirdboardDbContext>(connString);

                        var scope = services.BuildServiceProvider().CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<BirdboardDbContext>();
                        dbContext.Database.EnsureCreated();
                    }
                });
            }

            private static string? GetConnectionString()
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<IntegrationFixture>()
                    .Build();

                var connString = configuration.GetConnectionString("Test");

                return connString;
            }

            private static BirdboardDbContext CreateDbContext(IServiceCollection services)
            {
                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BirdboardDbContext>();
                return dbContext;
            }
        }
    }
}
