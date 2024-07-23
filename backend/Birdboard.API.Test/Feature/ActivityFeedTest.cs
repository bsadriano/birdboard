namespace Birdboard.API.Test.Feature;

public class ActivityFeedTest : AbstractIntegrationTest
{

    public ActivityFeedTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void CreatingAProjectRecordsActivity()
    {
        var project = await _projectFactory.Create(true);

        project.Activities.Count.Should().Be(1);
        project.Activities.First().Description.Should().Be("created");
    }



    [Fact]
    public async void UpdatingAProjectRecordsActivity()
    {
        var project = await _projectFactory.Create(true);

        project.Title = "Changed";

        await DbContext.SaveChangesAsync();

        project.Activities.Count.Should().Be(2);
        project.Activities.Last().Description.Should().Be("updated");
    }
}
