using System.Net.Http.Json;
using Birdboard.API.Models;
using Birdboard.API.Test.Fixtures;
using Birdboard.API.Test.Helper;
using FluentAssertions;

namespace Birdboard.API.Test.Feature;

public class ProjectsTest : IntegrationTest
{
    public ProjectsTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void AUserCanCreateAProject()
    {
        var newProject = DataFixture.GetProject();
        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await Client.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Project>>();
        result.Count().Should().Be(1);
        result.First().Title.Should().Be(newProject.Title);

        var project = DbContext.Projects.FirstOrDefault(p => p.Id == result.First().Id);
        project.Should().NotBeNull();
    }

    [Fact]
    public async void AUserCanViewAProject()
    {
        var newProject = DataFixture.GetProject();

        await DbContext.Projects.AddAsync(newProject);
        await DbContext.SaveChangesAsync();

        var response = await Client.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void AProjectRequiresATitle()
    {
        var newProject = DataFixture.GetProject();
        newProject.Title = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void AProjectRequiresADescription()
    {
        var newProject = DataFixture.GetProject();
        newProject.Description = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
