using System.Net.Http.Json;
using Birdboard.API.Models;
using Birdboard.API.Test.Fixtures;
using Birdboard.API.Test.Helper;
using FluentAssertions;

namespace Birdboard.API.Test.Feature;

[Collection(nameof(SharedTestCollection))]
public class ProjectsTest : BaseIntegrationTest
{
    public ProjectsTest(BirdboardWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async void AUserCanCreateAProject()
    {
        var newProject = DataFixture.GetProject();
        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await _client.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Project>>();
        result.Count().Should().BeGreaterThanOrEqualTo(1);
        result.Any(project => project.Title == newProject.Title);

        var project = _dbContext.Projects.FirstOrDefault(p => p.Title == newProject.Title);
        project.Should().NotBeNull();
    }

    [Fact]
    public async void AUserCanViewAProject()
    {
        var newProject = DataFixture.GetProject();

        await _dbContext.Projects.AddAsync(newProject);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void AProjectRequiresATitle()
    {
        var newProject = DataFixture.GetProject();
        newProject.Title = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void AProjectRequiresADescription()
    {
        var newProject = DataFixture.GetProject();
        newProject.Description = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
