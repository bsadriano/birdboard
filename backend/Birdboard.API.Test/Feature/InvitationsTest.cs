using Birdboard.API.Test.Helper;

namespace Birdboard.API.Test.Feature;

public class InvitationsTest : AbstractIntegrationTest
{
    public InvitationsTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void AProjectCanInviteAUser()
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
