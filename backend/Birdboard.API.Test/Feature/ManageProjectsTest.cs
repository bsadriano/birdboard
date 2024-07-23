using System.Net.Http.Json;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;

namespace Birdboard.API.Test.Feature;

public class ManageProjectsTest : AbstractIntegrationTest
{
    public ManageProjectsTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void GuestsCannotManageProjects()
    {
        var newProject = _projectFactory.GetProject().ToCreateProjectRequestDto();
        var httpContent = Http.BuildContent(newProject);

        Client.PostAsync(HttpHelper.Urls.Projects, httpContent)
            .Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var user = await _userFactory.Create();
        var project = await _projectFactory
            .WithOwner(user)
            .Create();

        Client.GetAsync(HttpHelper.Urls.Projects)
            .Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        Client.GetAsync(project.Path())
            .Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async void AUserCanCreateAProject()
    {
        await SignIn();

        var newProject = _projectFactory.GetProject().ToCreateProjectRequestDto();

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.Projects, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await Client.GetAsync(HttpHelper.Urls.Projects);
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
        await SignIn(user);

        var newProject = await _projectFactory
            .WithOwner(user)
            .Create();

        var response = await Client.GetAsync(newProject.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void AnAuthenticatedUserCannotViewTheProjectsOfOthers()
    {
        await SignIn();

        var user = await _userFactory.Create(true);

        var project = await _projectFactory
            .WithOwner(user)
            .Create();

        var response = await Client.GetAsync(project.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async void AProjectRequiresATitle()
    {
        await SignIn();

        var newProject = _projectFactory
            .GetProject()
            .ToCreateProjectRequestDto();

        newProject.Title = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.Projects, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void AProjectRequiresADescription()
    {
        await SignIn();

        var newProject = _projectFactory
            .GetProject()
            .ToCreateProjectRequestDto();

        newProject.Description = "";

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.Projects, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
