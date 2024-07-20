using Birdboard.API.Dtos.Project;
using Birdboard.API.Models;
using Bogus;

namespace Birdboard.API.Test.Factories;

public class ProjectFactory
{
    public AppUser? Owner { get; set; }

    public ProjectFactory WithOwner(AppUser user)
    {
        Owner = user;
        return this;
    }

    public List<Project> GetProjects(int count, bool useNewSeed = false)
    {
        return GetProjectFaker(useNewSeed).Generate(count);
    }

    public Project GetProject(bool useNewSeed = false)
    {
        return GetProjects(1, useNewSeed)[0];
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
            .RuleFor(t => t.Description, (faker, t) => faker.Name.Random.Words())
            .RuleFor(t => t.CreatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)))
            .RuleFor(t => t.UpdatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)));

        if (Owner != null)
        {
            faker.RuleFor(t => t.Owner, o => Owner);
        }

        return faker.UseSeed(seed);
    }
}
