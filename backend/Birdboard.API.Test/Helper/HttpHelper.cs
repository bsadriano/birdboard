namespace Birdboard.API.Test.Helper;

internal class HttpHelper
{
    internal static class Urls
    {
        private readonly static string BaseUrl = "/api/projects";
        public readonly static string Projects = BaseUrl;
        public static string GetProject(int id) => $"{BaseUrl}/{id}";
        public static string ProjectTasks(int id) => $"{BaseUrl}/{id}/tasks";
        public static string GetProjectTask(int projectId, int taskId) =>
            $"{BaseUrl}/{projectId}/tasks/{taskId}";
    }
}
