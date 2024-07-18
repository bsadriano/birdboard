namespace Birdboard.API.Test.Helper;

internal class HttpHelper
{
    internal static class Urls
    {
        private readonly static string BaseUrl = "/api/projects";
        public readonly static string GetProjects = BaseUrl;
        public static string GetProject(int id) => $"{BaseUrl}/{id}";
        public readonly static string AddProject = BaseUrl;
    }
}
