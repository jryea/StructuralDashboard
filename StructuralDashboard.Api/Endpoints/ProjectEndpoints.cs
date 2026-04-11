namespace StructuralDashboard.Api.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/projects");

        group.MapGet("/", GetAllProjects);
        group.MapGet("/{projectNumber}", GetProject);
        group.MapPost("/", CreateProject);
        group.MapPut("/{projectNumber}", UpdateProject);
        group.MapDelete("/{projectNumber}", DeleteProject);
    }

    private static async Task<IResult> DeleteProject(string projectNumber, IProjectService projectService)
    {
        await projectService.DeleteProjectAsync(projectNumber);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateProject(Project updated, IProjectService projectService)
    {
        await projectService.UpdateProjectAsync(updated);
        return Results.NoContent();
    }

    private static async Task<IResult> GetAllProjects(IProjectService projectService)
    {
        var projects = await projectService.GetAllProjectsAsync();
        return Results.Ok(projects);
    }

    private static async Task<IResult> GetProject(string projectNumber, IProjectService projectService)
    {
        var project = await projectService.GetProjectAsync(projectNumber);

        if (project != null)
        {
            return Results.Ok(project);
        }

        return Results.NotFound();
    }

    private static async Task<IResult> CreateProject(Project project, IProjectService projectService)
    {
        await projectService.CreateProjectAsync(project);

        return Results.Created($"/api/projects/{project.ProjectNumber}", project);
    }
}
