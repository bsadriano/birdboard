using Birdboard.API.Test.Fixtures;
using FluentAssertions;

namespace Birdboard.API.Test.Unit;

public class ProjectTest
{
    [Fact]
    public void ItHasAPath()
    {
        var project = DataFixture.GetProject();

        project.Path().Should().Be("/api/projects/0");
    }
}
