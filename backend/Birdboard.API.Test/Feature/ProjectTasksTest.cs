using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Birdboard.API.Mappers;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using FluentAssertions.Specialized;

namespace Birdboard.API.Test.Feature
{
    public class ProjectTasksTest : AbstractIntegrationTest
    {
        public ProjectTasksTest(IntegrationFixture integrationFixture) : base(integrationFixture)
        {
        }

        [Fact]
        public async void AProjectCanHaveTasks()
        {
            var user = await _userFactory.Create();

            await SignIn(user);

            var project = await _projectFactory
                .WithOwner(user)
                .Create();

            var projectTaskDto = _projectTaskFactory
                .GetProjectTask(true)
                .ToCreateProjectTaskRequestDto();

            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            response = await Client.GetAsync(HttpHelper.Urls.ProjectTasks(project.Id));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<ProjectTask>>();
            result.Count().Should().Be(1);
            result.First().Body.Should().Be(projectTaskDto.Body);

            var projectTask = DbContext.ProjectTasks.FirstOrDefault(p => p.Id == result.First().Id);
            projectTask.Should().NotBeNull();
        }

        [Fact]
        public async void AProjectRequiresADescription()
        {
            var user = await _userFactory.Create();

            await SignIn(user);

            var project = await _projectFactory
                .WithOwner(user)
                .Create();

            var projectTaskDto = _projectTaskFactory
                .GetProjectTask(true)
                .ToCreateProjectTaskRequestDto();
            projectTaskDto.Body = "";

            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void OnlyTheOwnerOfAProjectMayAddTasks()
        {
            var user = await _userFactory.Create();

            await SignIn(user);

            var otherUser = await _userFactory.Create();
            var project = await _projectFactory
                .WithOwner(otherUser)
                .Create();

            var projectTaskDto = _projectTaskFactory
                .GetProjectTask(true)
                .ToCreateProjectTaskRequestDto();

            var httpContent = Http.BuildContent(projectTaskDto);
            var response = await Client.PostAsync(HttpHelper.Urls.ProjectTasks(project.Id), httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);

        }
    }
}
