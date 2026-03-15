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
        // Get ProjectEntity from repository
        var entity = await _repository.GetProjectAsync(projectNumber);
        if (entity is null) return null;

        // Map ProjectEntity to Project domain model
        return new Project()
        {
            ProjectNumber = entity.ProjectNumber,
            ProjectName = entity.ProjectName
        };
    }

    public Task CreateProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProjectAsync(string projectNumber)
    {
        throw new NotImplementedException();
    }


    public Task<List<Project>> GetProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }
}
