using System.Net.Http.Json;
using Birdboard.API.Dtos.ProjectTask;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using Microsoft.EntityFrameworkCore;

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

    [Fact]
    public async void CreatingANewTaskRecordsProjectActivity()
    {
        var project = await _projectFactory.Create(true);

        var task = _projectTaskFactory.GetProjectTask(true);

        project.Tasks.Add(task);

        await DbContext.SaveChangesAsync();

        project.Activities.Count.Should().Be(2);
        project.Activities.Last().Description.Should().Be("created_task");
    }

    [Fact]
    public async void CompletingATaskRecordsProjectActivity()
    {
        var project = await _projectFactory.WithTasks(1).Create(true);

        await SignIn(project.Owner);

        var updateTaskDto = new UpdateProjectTaskRequestDto
        {
            Completed = true
        };

        var path = project.Tasks.First().Path();
        var httpContent = Http.BuildContent(updateTaskDto);
        var response = await Client.PatchAsync(path, httpContent);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<object>();

        DbContext.Entry(project).State = EntityState.Detached;

        var updatedProject = await DbContext.Projects
            .Include(p => p.Activities)
            .FirstOrDefaultAsync(i => i.Id == project.Id);

        updatedProject.Activities.Count.Should().Be(3);
        updatedProject.Activities.Last().Description.Should().Be("completed_task");
    }
}
