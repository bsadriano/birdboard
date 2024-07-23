using Birdboard.API.Data;
using Birdboard.API.Models;
using Bogus;

namespace Birdboard.API.Test.Factories;

public class ProjectFactory
{
    public BirdboardDbContext DbContext { get; set; }
    public AppUser? Owner { get; set; }
    public int TasksCount = 0;

    public ProjectFactory(BirdboardDbContext dbContext = null)
    {
        DbContext = dbContext;
    }

    public ProjectFactory OwnedBy(AppUser user)
    {
        Owner = user;
        return this;
    }

    public ProjectFactory WithTasks(int count)
    {
        TasksCount = count;
        return this;
    }

    public List<Project> GetProjects(int count, bool useNewSeed = false)
    {
        var faker = GetProjectFaker(useNewSeed);
        return faker.Generate(count);
    }

    public Project GetProject(bool useNewSeed = false)
    {
        var projects = GetProjects(1, useNewSeed);
        return projects[0];
    }

    public async Task<List<Project>> Create(int count, bool useNewSeed = false)
    {
        var newProjects = GetProjects(count, useNewSeed);

        var projectTaskFactory = new ProjectTaskFactory();

        if (TasksCount > 0)
        {
            foreach (var project in newProjects)
            {
                var tasks = projectTaskFactory.GetProjectTasks(TasksCount, true);
                project.Tasks = tasks;
            }
        }

        await DbContext.Projects.AddRangeAsync(newProjects);

        await DbContext.SaveChangesAsync();

        return newProjects;
    }

    private Faker<Project> GetProjectFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }

        var faker = new Faker<Project>()
            .RuleFor(t => t.Id, o => 0)
            .RuleFor(t => t.Title, (faker, t) => faker.Name.Random.Word())
            .RuleFor(t => t.Notes, (faker, t) => faker.Name.Random.Words())
            .RuleFor(t => t.Description, (faker, t) => faker.Name.Random.Words())
            .RuleFor(t => t.CreatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)))
            .RuleFor(t => t.UpdatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)));

        if (Owner != null)
            faker.RuleFor(t => t.Owner, o => Owner);

        return faker.UseSeed(seed);
    }
}
