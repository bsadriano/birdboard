using System.Net.Http.Json;
using System.Text;
using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Birdboard.API.Test.Feature;

public class ProjectsTest
{
    [Fact]
    public async void AUserCanCreateAProject()
    {
        // Arrange
        var _factory = new BirdboardWebApplicationFactory();

        using (var scope = _factory.Services.CreateScope())
        {
            var scopeService = scope.ServiceProvider;
            var dbContext = scopeService.GetRequiredService<BirdboardDbContext>();

            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }

        var cilent = _factory.CreateClient();

        // Act
        var newProject = new Project()
        {
            Title = "Some Title",
            Description = "Some Description",
        };
        var httpContent = new StringContent(JsonConvert.SerializeObject(newProject), Encoding.UTF8, "application/json");
        var request = await cilent.PostAsync(HttpHelper.Urls.AddProject, httpContent);

        // Assert
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await cilent.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Project>>();
        result.Count.Should().Be(1);
        result[0].Title.Should().Be(newProject.Title);
    }
}
