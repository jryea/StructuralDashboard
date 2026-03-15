namespace StructuralDashboard.Api.Services;

public interface IModelService
{
    Task<List<Project>> GetAllProjectsAsync()
}
