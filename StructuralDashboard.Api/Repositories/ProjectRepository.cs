namespace StructuralDashboard.Api.Repositories;

// Repositories translate models to entities

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateProjectAsync(Project project)
    {
        var entity = new ProjectEntity()
        {
            ProjectNumber = project.ProjectNumber,
            ProjectName = project.ProjectName
        };

        // Add project to DB
        _context.Projects.Add(entity);

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

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        // Get all projects from db
        var projects = await _context.Projects.ToListAsync();

        return projects.Select(x => new Project()
        {
            ProjectName = x.ProjectName,
            ProjectNumber = x.ProjectNumber
        }).ToList();
    }
    

    public async Task<Project?> GetProjectAsync(string projectNumber)
    {
        // get project from db
        var entity = await _context.Projects.FindAsync(projectNumber);

        // return project from db
        if (entity is null) return null;

        return new Project()
        {
            ProjectNumber = entity.ProjectNumber,
            ProjectName = entity.ProjectName
        };
    }

    public async Task UpdateProjectAsync(Project project)
    {
        // Get project number
        var projectNumber = project.ProjectNumber;

        // Get project
        var entity = await _context.Projects.FindAsync(projectNumber);

        // Update project
        if (entity is null) return;
        entity.ProjectName = project.ProjectName;

        // Save db w changes
        await _context.SaveChangesAsync();
    }
}
