using System.Net.Http.Headers;

namespace Birdboard.API.Test;

public static class HttpClientExtensions
{
    public static HttpClient WithJwtBearerToken(this HttpClient client, Action<TestJwtToken> configure)
    {
        var token = new TestJwtToken();
        configure(token);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Build());
        return client;
    }
}
