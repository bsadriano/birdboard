using Birdboard.API.Models;
using Birdboard.API.Test.Factories;

namespace Birdboard.API.Test.Unit;

public class ProjectTest
{
    [Fact]
    public async void ItHasAPath()
    {
        Project project = new ProjectFactory().GetProject();
        project.Path().Should().Be("/api/projects/0");
    }
}
