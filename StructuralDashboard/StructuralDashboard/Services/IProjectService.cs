

namespace StructuralDashboard.Api.Services;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync();
    Task CreateAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(string projectNumber);
}
