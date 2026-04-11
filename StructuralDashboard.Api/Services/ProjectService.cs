namespace StructuralDashboard.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }
    public async Task<Project?> GetProjectAsync(string projectNumber)
    {
        return await _repository.GetProjectAsync(projectNumber);
    }

    public async Task CreateProjectAsync(Project project)
    {
       await _repository.CreateProjectAsync(project);
    }

    public async Task DeleteProjectAsync(string projectNumber)
    {
        await _repository.DeleteProjectAsync(projectNumber);
    }


    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _repository.GetAllProjectsAsync();
    }

    public async Task UpdateProjectAsync(Project project)
    {
        await _repository.UpdateProjectAsync(project);  
    }
}
