namespace StructuralDashboard.Api.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateProjectAsync(ProjectEntity project)
    {
        // Add project to DB
        _context.Projects.Add(project);

        // Save changes to Db
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(string projectNumber)
    {
        // Get project from db
        var project = await _context.Projects.FindAsync(projectNumber);
        if (project is null) return;

        // Remove project from Db
        _context.Projects.Remove(project);

        // Save changes to Db
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProjectEntity>> GetProjectsAsync()
    {
        // Get all projects from db
        var projects = await _context.Projects.ToListAsync();

        // Return projects or empty list if null
        return projects;
    }

    public async Task<ProjectEntity?> GetProjectAsync(string projectNumber)
    {
        // get project from db
        var project = await _context.Projects.FindAsync(projectNumber);

        // return project from db
        return project;
    }

    public async Task UpdateProjectAsync(ProjectEntity updatedProject)
    {
        // Get project number
        var projectNumber = updatedProject.ProjectNumber;

        // Get project
        var project = await _context.Projects.FindAsync(projectNumber);

        // Update project
        project?.ProjectName = updatedProject.ProjectName;

        // Save db w changes
        await _context.SaveChangesAsync();
    }
}
