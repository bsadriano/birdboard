using System.Net.Http.Json;
using Birdboard.API.Dtos.ProjectInvitation;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using Microsoft.EntityFrameworkCore;

namespace Birdboard.API.Test.Feature;

public class InvitationsTest : AbstractIntegrationTest
{
    public InvitationsTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void NonOwnersMayNotInviteUsers()
    {
        var project = await _projectFactory
            .Create(true);
        var user = await _userFactory.Create(true);
        var userToInvite = await _userFactory.Create(true);

        await SignIn(user);

        var httpContent = Http.BuildContent(new ProjectInvitationRequestDto
        {
            Email = userToInvite.Email
        });
        var response = await Client.PostAsync(project.Path() + "/invitations", httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

        await DbContext.ProjectMembers.AddAsync(new ProjectMember
        {
            UserId = user.Id,
            ProjectId = project.Id
        });
        await DbContext.SaveChangesAsync();

        response = await Client.PostAsync(project.Path() + "/invitations", httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async void AProjectCanInviteAUser()
    {
        var project = await _projectFactory
            .Create(true);

        await SignIn(project.Owner);

        var userToInvite = await _userFactory.Create(true);

        var httpContent = Http.BuildContent(new ProjectInvitationRequestDto
        {
            Email = userToInvite.Email
        });
        var response = await Client.PostAsync(project.Path() + "/invitations", httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        DbContext.ProjectMembers.CountAsync().Result.Should().Be(1);
    }

    [Fact]
    public async void TheEmailAddressMustBeAssociatedWithAValidBirdboardAccount()
    {
        var project = await _projectFactory
            .Create(true);

        await SignIn(project.Owner);

        var httpContent = Http.BuildContent(new ProjectInvitationRequestDto
        {
            Email = "invalid@email.com"
        });
        var response = await Client.PostAsync(project.Path() + "/invitations", httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Errors.Should().ContainKey("email");
        result.Errors["email"].Should().Be("The user you are inviting must have a Birdboard account");
    }

    [Fact]
    public async void InvitedUsersMayUpdateProjectDetails()
    {
        var user = await _userFactory.Create();
        var project = await _projectFactory
            .WithInvitees(user)
            .WithTasks(1)
            .Create();

        await SignIn(user);
        var httpContent = Http.BuildContent(new { body = "Foo task" });
        var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }
}
