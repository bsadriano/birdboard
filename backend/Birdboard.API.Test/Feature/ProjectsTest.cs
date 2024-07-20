using System.Net.Http.Json;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Test.Factories;
using Birdboard.API.Test.Helper;

namespace Birdboard.API.Test.Feature;

public class ProjectsTest : IntegrationTest
{
    public ProjectsTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void GuestsCannotCreateAProject()
    {
        var newProject = _projectFactory.GetProject().ToCreateProjectRequestDto();

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void GuestsMayNotViewProjects()
    {
        var request = await Client.GetAsync(HttpHelper.Urls.GetProjects);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void GuestsCannotViewASingleProject()
    {
        var user = await _userFactory.Create();
        var project = await _projectFactory
            .WithOwner(user)
            .Create();

        var request = await Client.GetAsync(project.Path());

        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void AUserCanCreateAProject()
    {
        var user = await _userFactory.Create();
        LoginAs(user);

        var newProject = _projectFactory.GetProject().ToCreateProjectRequestDto();

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
    public async void AUserCanViewTheirProject()
    {
        var user = await _userFactory.Create(true);
        LoginAs(user);

        var newProject = await _projectFactory
            .WithOwner(user)
            .Create();

        var response = await Client.GetAsync(newProject.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void AnAuthenticatedUserCannotViewTheProjectsOfOthers()
    {
        var user = await _userFactory.Create(true);
        var otherUser = await _userFactory.Create(true);
        LoginAs(user);

        var project = await _projectFactory
            .WithOwner(otherUser)
            .Create();

        var response = await Client.GetAsync(project.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void AProjectRequiresATitle()
    {
        var user = await _userFactory.Create();
        LoginAs(user);

        var newProject = _projectFactory
            .WithOwner(user)
            .GetProject()
            .ToCreateProjectRequestDto();

        newProject.Title = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void AProjectRequiresADescription()
    {
        var user = await _userFactory.Create();
        LoginAs(user);

        var newProject = _projectFactory
            .WithOwner(user)
            .GetProject()
            .ToCreateProjectRequestDto();

        newProject.Description = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
