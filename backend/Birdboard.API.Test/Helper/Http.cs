using System.Text;
using Newtonsoft.Json;

namespace Birdboard.API.Test.Helper;

public static class Http
{
    public static StringContent BuildContent(object value)
    {
        return new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
    }
}
