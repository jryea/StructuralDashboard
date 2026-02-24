namespace StructuralDashboard.Api;

public static class ApiEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Welcome to the Structural Dashboard API");
        // Use map group later on to group endpoints together
        app.MapGet("api/projects", GetAllProjects);
        app.MapGet("api/projects/{id}", GetProject);
        app.MapPost("api/projects", CreateProject);
    }

    private static string GetAllProjects()
    {
        return "This is the get all projects endpoint";
    }

    private static string GetProject(int id)
    {
        return $"This is the get project endpoint for project {id}";
    }

    private static string CreateProject(int id) 
    {
        return $"Creating project {id}";
    }
}
