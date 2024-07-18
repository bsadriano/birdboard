using System.Net.Http.Json;
using Birdboard.API.Data;
using Birdboard.API.Models;
using Birdboard.API.Test.Helper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Birdboard.API.Test.Feature;

public class ProjectsTest
{
    BirdboardWebApplicationFactory _factory;
    HttpClient _client;

    public ProjectsTest()
    {
        _factory = new BirdboardWebApplicationFactory();

        using (var scope = _factory.Services.CreateScope())
        {
            var scopeService = scope.ServiceProvider;
            var dbContext = scopeService.GetRequiredService<BirdboardDbContext>();

            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }

        _client = _factory.CreateClient();
    }
    [Fact]
    public async void AUserCanCreateAProject()
    {
        var newProject = new Project
        {
            Title = "Some Title",
            Description = "Some Description",
        };
        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var response = await _client.GetAsync(HttpHelper.Urls.GetProjects);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Project>>();
        result.Count().Should().Be(1);
        result[0].Title.Should().Be(newProject.Title);
    }

    [Theory, MemberData(nameof(RequiredData))]
    public async void AProjectRequiresATitleAndADescription(string title, string description)
    {
        var newProject = new Project
        {
            Title = title,
            Description = description
        };
        var httpContent = Http.BuildContent(newProject);
        var request = await _client.PostAsync(HttpHelper.Urls.AddProject, httpContent);
        request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    private static IEnumerable<object[]> RequiredData =>
        new List<object[]>
        {
            new object[] { "Some Title", "" },
            new object[] { "", "Some Description" },
        };
}
