using System.Net.Http.Json;
using Birdboard.API.Dtos.Project;
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

        var project = await _projectFactory.Create();

        project.Title = "Changed";
        project.Description = "Changed";
        project.Notes = "Changed";
        httpContent = Http.BuildContent(project.ToCreateProjectRequestDto());
        Client.PatchAsync(project.Path(), httpContent)
            .Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        Client.GetAsync(HttpHelper.Urls.Projects)
            .Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        Client.GetAsync(project.Path())
            .Result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async void AUserCanCreateAProject()
    {
        await SignIn();

        var newProject = _projectFactory.GetProject(true).ToCreateProjectRequestDto();

        var httpContent = Http.BuildContent(newProject);
        var request = await Client.PostAsync(HttpHelper.Urls.Projects, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await Client.GetAsync(HttpHelper.Urls.Projects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
        result.Count().Should().Be(1);
        result.First().Title.Should().Be(newProject.Title);

        var project = DbContext.Projects.FirstOrDefault(p => p.Id == result.First().Id);
        project.Should().NotBeNull();
        project.Title.Should().Be(newProject.Title);
        project.Description.Should().Be(newProject.Description);
        project.Notes.Should().Be(newProject.Notes);
    }

    [Fact]
    public async void AUserCanSeeAlProjectsTheyHaveBeenInvitedToOnTheirDashboard()
    {
        var user = await _userFactory.Create();
        var project = await _projectFactory
            .WithInvitees(user)
            .Create();

        await SignIn(user);

        var response = await Client.GetAsync(HttpHelper.Urls.Projects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
        result.Count().Should().Be(1);
        result.First().Title.Should().Be(project.Title);
    }

    [Fact]
    public async void UnauthorizedUsersCannotDeleteProjects()
    {
        var newProject = await _projectFactory.Create();

        var response = await Client.DeleteAsync(newProject.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        await SignIn();

        response = await Client.DeleteAsync(newProject.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async void AUserCanDeleteAProject()
    {
        var newProject = await _projectFactory.Create();

        await SignIn(newProject.Owner);

        var response = await Client.DeleteAsync(newProject.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var project = DbContext.Projects.FirstOrDefault(p => p.Id == newProject.Id);
        project.Should().BeNull();
    }

    [Fact]
    public async void AUserCanUpdateAProject()
    {
        var newProject = await _projectFactory.Create();

        await SignIn(newProject.Owner);

        newProject.Notes = "Changed";

        var httpContent = Http.BuildContent(newProject.ToCreateProjectRequestDto());
        var response = await Client.PatchAsync(newProject.Path(), httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var project = DbContext.Projects.FirstOrDefault(p => p.Notes == newProject.Notes);
        project.Should().NotBeNull();
    }

    [Fact]
    public async void AUserCanUpdateAProjectsGeneralNotes()
    {
        var newProject = await _projectFactory.Create();

        await SignIn(newProject.Owner);

        var updatedNotes = new CreateProjectRequestDto();
        updatedNotes.Notes = "Changed";

        var httpContent = Http.BuildContent(updatedNotes);
        var response = await Client.PatchAsync(newProject.Path(), httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var project = DbContext.Projects.FirstOrDefault(p => p.Notes == updatedNotes.Notes);
        project.Should().NotBeNull();
    }

    [Fact]
    public async void AUserCanViewTheirProject()
    {
        var newProject = await _projectFactory.Create();

        await SignIn(newProject.Owner);

        var response = await Client.GetAsync(newProject.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void AnAuthenticatedUserCannotViewTheProjectsOfOthers()
    {
        await SignIn();

        var project = await _projectFactory.Create();

        var response = await Client.GetAsync(project.Path());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async void AnAuthenticatedUserCannotUpdateTheProjectsOfOthers()
    {
        await SignIn();

        var project = await _projectFactory.Create();

        project.Notes = "Changed";

        var httpContent = Http.BuildContent(project.ToCreateProjectRequestDto());
        var response = await Client.PatchAsync(project.Path(), httpContent);
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
