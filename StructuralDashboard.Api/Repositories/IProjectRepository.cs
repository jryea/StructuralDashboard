namespace StructuralDashboard.Api.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectAsync(string projectNumber);
    Task CreateProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(string projectNumber);
}
