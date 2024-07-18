using Birdboard.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Birdboard.API.Test;

public class BirdboardWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<BirdboardDbContext>));

            var connString = GetConnectionString();
            services.AddSqlServer<BirdboardDbContext>(connString);

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
        });
    }

    private static string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Test.json", false, true)
            .Build();

        var connString = configuration.GetConnectionString("TestConnection");

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
