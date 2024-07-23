using Birdboard.API.Data;
using Birdboard.API.Models;
using Bogus;

namespace Birdboard.API.Test.Factories;

public class ProjectTaskFactory
{
    public BirdboardDbContext DbContext { get; set; }
    public Project? Project { get; set; }

    public ProjectTaskFactory(BirdboardDbContext dbContext = null)
    {
        DbContext = dbContext;
    }

    public ProjectTaskFactory WithProject(Project project)
    {
        Project = project;
        return this;
    }

    public List<ProjectTask> GetProjectTasks(int count, bool useNewSeed = false)
    {
        return GetProjectTaskFaker(useNewSeed).Generate(count);
    }

    public ProjectTask GetProjectTask(bool useNewSeed = false)
    {
        return GetProjectTasks(1, useNewSeed)[0];
    }

    public async Task<List<ProjectTask>> Create(int count, bool useNewSeed = false)
    {
        var newProjectTasks = GetProjectTasks(count, useNewSeed);
        await DbContext.ProjectTasks.AddRangeAsync(newProjectTasks);
        await DbContext.SaveChangesAsync();

        return newProjectTasks;
    }

    private Faker<ProjectTask> GetProjectTaskFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }

        var faker = new Faker<ProjectTask>()
            .RuleFor(t => t.Id, o => 0)
            .RuleFor(t => t.Body, (faker, t) => faker.Name.Random.Words())
            .RuleFor(t => t.Completed, (faker, t) => faker.Random.Int(0, 100) % 2 == 0)
            .RuleFor(t => t.CreatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)))
            .RuleFor(t => t.UpdatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)));

        if (Project != null)
            faker.RuleFor(t => t.Project, o => Project);

        return faker.UseSeed(seed);
    }
}
