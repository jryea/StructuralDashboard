namespace StructuralDashboard.Api.Endpoints;

// We have access to the dependency injection container through the WebApplication instance, so we can inject our DbContext directly into the method parameters.

public static class ApiEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/projects");

        group.MapGet("/", GetAllProjects);
        group.MapGet("/{projectNumber}", GetProject);
        group.MapPost("/", CreateProject);
        group.MapPut("/{projectNumber}", UpdateProject); 
        group.MapDelete("/{projectNumber}", DeleteProject);  
    }

    private static async Task<IResult> DeleteProject(string projectNumber, AppDbContext db)
    {
        // Get project
        var project = await db.Projects.FindAsync(projectNumber);
        if (project is null) return Results.NotFound();

        // Remove project
        db.Projects.Remove(project);

        // Save changes
        await db.SaveChangesAsync();

        // Return results
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateProject(string projectNumber, Project updated, AppDbContext db)
    {
        // Get project
        var project = await db.Projects.FindAsync(projectNumber);
        if (project is null) return Results.NotFound();

        // Update project
        project.ProjectName = updated.ProjectName;

        // save project
        await db.SaveChangesAsync();

        // return results
        // 204 NoContent is the convention for a succcessful update w nothing to return
        return Results.NoContent();
    }

    private static async Task<IResult> GetAllProjects(AppDbContext db)
    {
        var projects = await db.Projects.ToListAsync();
        return Results.Ok(projects);
    }

    private static async Task<IResult> GetProject(string projectNumber, AppDbContext db)
    {
        var project = await db.Projects.FindAsync(projectNumber);

        if (project != null)
        {
            return Results.Ok(project);
        }

         return Results.NotFound();
    }

    private static async Task<IResult> CreateProject(Project project, AppDbContext db) 
    {
        // add the project to the DB, similiar to Git staging
        db.Projects.Add(project);

        // save the changes to the DB, commits to the DB
        await db.SaveChangesAsync();

        return Results.Created($"/api/projects/{project.ProjectNumber}", project);
    }
}
