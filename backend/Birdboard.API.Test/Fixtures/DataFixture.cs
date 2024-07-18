using Birdboard.API.Models;
using Bogus;

namespace Birdboard.API.Test.Fixtures;

public class DataFixture
{
    public static List<Project> GetProjects(int count, bool useNewSeed = false)
    {
        return GetProjectFaker(useNewSeed).Generate(count);
    }

    public static Project GetProject(bool useNewSeed = false)
    {
        return GetProjects(1, useNewSeed)[0];
    }

    private static Faker<Project> GetProjectFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }

        return new Faker<Project>()
            .RuleFor(t => t.Id, o => 0)
            .RuleFor(t => t.Title, (faker, t) => faker.Name.Random.Word())
            .RuleFor(t => t.Description, (faker, t) => faker.Name.Random.Words())
            .RuleFor(t => t.CreatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)))
            .RuleFor(t => t.UpdatedAt, (faker, t) => faker.Date.Past(5, new DateTime(2020, 1, 1)))
            .UseSeed(seed);
    }
}
