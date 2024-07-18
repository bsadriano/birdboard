using System.Net.Http.Json;
using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test.Feature;

public class ProjectsTest : BaseIntegrationTest
{
    public ProjectsTest(BirdboardWebApplicationFactory factory)
        : base(factory)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var scopeService = scope.ServiceProvider;
            var dbContext = scopeService.GetRequiredService<BirdboardDbContext>();

            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }
    }
    [Fact]
    public async void AUserCanCreateAProject()
    {
        var newProject = new Project
        {
            Title = "Some Title",
            Description = "Some Description",
        };
        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await _client.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Project>>();
        result.Count().Should().Be(1);
        result[0].Title.Should().Be(newProject.Title);

        _dbContext.Projects.FirstOrDefault(p => p.Id == newProject.Id);
    }

    [Fact]
    public async void AProjectRequiresATitle()
    {
        var newProject = new Project
        {
            Title = "Some Title",
            Description = ""
        };
        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void AProjectRequiresADescription()
    {
        var newProject = new Project
        {
            Title = "",
            Description = "Some Description"
        };
        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
