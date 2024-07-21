using Birdboard.API.Test.Factories;

namespace Birdboard.API.Test.Unit;

public class ProjectTaskTest
{
    [Fact]
    public void ItHasAPath()
    {
        var projectTask = new ProjectTaskFactory().GetProjectTask();

        projectTask.Path().Should().Be("/api/projects/0/tasks/0");
    }
}
