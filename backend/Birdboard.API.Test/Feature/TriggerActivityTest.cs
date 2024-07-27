using System.Net.Http.Json;
using Birdboard.API.Dtos.ProjectTask;
using Birdboard.API.Test.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Birdboard.API.Test.Feature;

public class TriggerActivityTest : AbstractIntegrationTest
{

    public TriggerActivityTest(IntegrationFixture integrationFixture) : base(integrationFixture)
    {
    }

    [Fact]
    public async void CreatingAProject()
    {
        await _projectFactory.Create(true);

        DbContext.Activities.Count().Should().Be(1);
        var activity = DbContext.Activities.Last();
        activity.Description.Should().Be("created");
        activity.Changes.Should().BeNullOrEmpty();
    }

    [Fact]
    public async void UpdatingAProject()
    {
        var project = await _projectFactory.Create(true);
        var originalTitle = project.Title;
        project.Title = "Changed";

        await DbContext.SaveChangesAsync();

        DbContext.Activities.Count().Should().Be(2);
        var last = await DbContext.Activities.OrderBy(a => a.Id).LastAsync();
        last.Description.Should().Be("updated");
        last.Changes.Should().ContainAll("before", "after");
        JObject jObject = JsonConvert.DeserializeObject<dynamic>(last.Changes);

        jObject["before"]["title"].ToString().Should().Be(originalTitle);
        jObject["after"]["title"].ToString().Should().Be("Changed");
    }

    [Fact]
    public async void CreatingANewTask()
    {
        var project = await _projectFactory.Create(true);

        var task = _projectTaskFactory.GetProjectTask(true);

        project.Tasks.Add(task);

        await DbContext.SaveChangesAsync();

        DbContext.Activities.Count().Should().Be(2);
        var last = await DbContext.Activities.OrderBy(a => a.Id).LastAsync();
        last.Description.Should().Be("created_task");
        last.SubjectType.Should().Be("ProjectTask");
    }

    [Fact]
    public async void CompletingATask()
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

        DbContext.Activities.Count().Should().Be(3);
        var last = await DbContext.Activities.OrderBy(a => a.Id).LastAsync();
        last.Description.Should().Be("completed_task");
    }

    [Fact]
    public async void IncompletingATask()
    {
        var project = await _projectFactory.WithTasks(1).Create(true);

        await SignIn(project.Owner);

        var path = project.Tasks.First().Path();
        var response = await Client.PatchAsync(
            path,
            Http.BuildContent(
                new UpdateProjectTaskRequestDto
                {
                    Completed = true
                }
            )
        );
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        DbContext.Entry(project).State = EntityState.Detached;

        DbContext.Activities.Count().Should().Be(3);

        var last = await DbContext.Activities.OrderBy(a => a.Id).LastAsync();
        last.Description.Should().Be("completed_task");

        // Incompleting task

        response = await Client.PatchAsync(
            path,
            Http.BuildContent(
                new UpdateProjectTaskRequestDto
                {
                    Completed = false
                }
            )
        );
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        DbContext.Activities.Count().Should().Be(4);
        last = await DbContext.Activities.OrderBy(a => a.Id).LastAsync();
        last.Description.Should().Be("incompleted_task");
    }

    [Fact]
    public async void DeleteATask()
    {
        var project = await _projectFactory.WithTasks(1).Create(true);

        DbContext.ProjectTasks.Remove(project.Tasks.First());

        await DbContext.SaveChangesAsync();

        DbContext.Activities.Count().Should().Be(3);

        var last = await DbContext.Activities.OrderBy(a => a.Id).LastAsync();
        last.Description.Should().Be("deleted_task");
    }
}
