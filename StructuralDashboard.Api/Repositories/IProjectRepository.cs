namespace StructuralDashboard.Api.Repositories;

public interface IProjectRepository
{
    Task<List<ProjectEntity>> GetProjectsAsync();
    Task<ProjectEntity?> GetProjectAsync(string projectNumber);
    Task CreateProjectAsync(ProjectEntity project);
    Task UpdateProjectAsync(ProjectEntity project);
    Task DeleteProjectAsync(string projectNumber);
}
