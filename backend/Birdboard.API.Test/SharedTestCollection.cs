namespace Birdboard.API.Test;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<BirdboardWebApplicationFactory>
{
}
