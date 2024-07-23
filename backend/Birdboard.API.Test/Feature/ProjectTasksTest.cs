using System.Net.Http.Json;
using Birdboard.API.Dtos.ProjectTask;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;

namespace Birdboard.API.Test.Feature
{
    public class ProjectTasksTest : AbstractIntegrationTest
    {
        public AppUser user { get; set; }
        public Project project { get; set; }
        public CreateProjectTaskRequestDto projectTaskDto { get; set; }

        public ProjectTasksTest(IntegrationFixture integrationFixture) : base(integrationFixture)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            user = await _userFactory.Create();

            await SignIn(user);

            project = await _projectFactory
                .OwnedBy(user)
                .WithTasks(1)
                .Create();

            projectTaskDto = _projectTaskFactory
                .GetProjectTask(true)
                .ToCreateProjectTaskRequestDto();
        }

        [Fact]
        public async void AProjectCanHaveTasks()
        {
            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            response = await Client.GetAsync(HttpHelper.Urls.ProjectTasks(project.Id));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<ProjectTask>>();
            result.Count().Should().BeGreaterThanOrEqualTo(1);
            result[1].Body.Should().Be(projectTaskDto.Body);

            var projectTask = DbContext.ProjectTasks.FirstOrDefault(p => p.Id == result[1].Id);
            projectTask.Should().NotBeNull();
        }

        [Fact]
        public async void ATaskCanBeUpdated()
        {
            var projectTask = project.Tasks.First();

            var updateProjectTaskDto = new UpdateProjectTaskRequestDto
            {
                Body = "Changed"
            };

            var httpContent = Http.BuildContent(updateProjectTaskDto);
            var response = await Client.PatchAsync(projectTask.Path(), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var updatedTask = DbContext.ProjectTasks.FirstOrDefault(p => p.Body == updateProjectTaskDto.Body);
            updatedTask.Should().NotBeNull();
        }

        [Fact]
        public async void ATaskCanBeCompleted()
        {
            var projectTask = project.Tasks.First();

            var updateProjectTaskDto = new UpdateProjectTaskRequestDto
            {
                Completed = true
            };

            var httpContent = Http.BuildContent(updateProjectTaskDto);
            var response = await Client.PatchAsync(projectTask.Path(), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var updatedTask = DbContext.ProjectTasks.FirstOrDefault(p => p.Id == projectTask.Id);
            updatedTask.Should().NotBeNull();
        }

        [Fact]
        public async void ATaskCanBeMarkedAsIncomplete()
        {
            var projectTask = project.Tasks.First();

            var updateProjectTaskDto = new UpdateProjectTaskRequestDto
            {
                Completed = true
            };

            var httpContent = Http.BuildContent(updateProjectTaskDto);
            await Client.PatchAsync(projectTask.Path(), httpContent);

            updateProjectTaskDto = new UpdateProjectTaskRequestDto
            {
                Completed = false
            };

            httpContent = Http.BuildContent(updateProjectTaskDto);
            var response = await Client.PatchAsync(projectTask.Path(), httpContent);

            var updatedTask = DbContext.ProjectTasks.FirstOrDefault(p => p.Id == projectTask.Id);
            updatedTask.Should().NotBeNull();
            updatedTask.Completed.Should().Be(false);
        }

        [Fact]
        public async void AProjectRequiresABody()
        {
            projectTaskDto.Body = "";

            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void OnlyTheOwnerOfAProjectMayAddTasks()
        {
            var otherUser = await _userFactory.Create();
            project = await _projectFactory
                .OwnedBy(otherUser)
                .Create();

            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async void OnlyTheOwnerOfAProjectMayUpdateATask()
        {
            var otherUser = await _userFactory.Create();
            var project = await _projectFactory
                .OwnedBy(otherUser)
                .WithTasks(1)
                .Create();

            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PatchAsync(project.Tasks.First().Path(), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

            var updatedTask = DbContext.ProjectTasks.FirstOrDefault(p => p.Body == projectTaskDto.Body);
            updatedTask.Should().BeNull();
        }
    }
}
