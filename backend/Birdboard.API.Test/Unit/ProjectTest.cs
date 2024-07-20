using Birdboard.API.Test.Factories;

namespace Birdboard.API.Test.Unit;

public class ProjectTest
{
    [Fact]
    public void ItHasAPath()
    {
        var project = new ProjectFactory().GetProject();

        project.Path().Should().Be("/api/projects/0");
    }
}
